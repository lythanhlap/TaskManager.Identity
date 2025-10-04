using Microsoft.EntityFrameworkCore;
using TaskManager.Identity.Abstractions;
using TaskManager.Identity.Persistence.EFCore;
using TaskManager.Identity.Persistence.EFCore.Entities;

namespace TaskManager.Identity.Core.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IdentityDbContext _db;
    private readonly Token.JwtTokenFactory _jwt;

    public AuthService(IdentityDbContext db, Token.JwtTokenFactory jwt)
    { _db = db; _jwt = jwt; }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        // Kiểm tra trùng username & email
        if (await _db.Users.AnyAsync(x => x.Username == req.Username, ct))
            throw new InvalidOperationException("Username already exists");
        if (await _db.Users.AnyAsync(x => x.Email == req.Email, ct))
            throw new InvalidOperationException("Email already exists");

        var u = new ApplicationUser
        {
            Email = req.Email,
            Username = req.Username,
            FullName = req.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };
        _db.Users.Add(u);
        await _db.SaveChangesAsync(ct);
        return _jwt.Create(u);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        // Cho phép nhập username hoặc email
        var isEmail = req.Identifier.Contains("@");
        ApplicationUser? u = isEmail
            ? await _db.Users.SingleOrDefaultAsync(x => x.Email == req.Identifier, ct)
            : await _db.Users.SingleOrDefaultAsync(x => x.Username == req.Identifier, ct);

        if (u is null || !BCrypt.Net.BCrypt.Verify(req.Password, u.PasswordHash))
            throw new InvalidOperationException("Invalid credentials");

        return _jwt.Create(u);
    }

    public async Task<bool> ChangePasswordAsync(string userId, string oldPwd, string newPwd, CancellationToken ct = default)
    {
        var u = await _db.Users.FindAsync(new object[] { userId }, ct);
        if (u is null || !BCrypt.Net.BCrypt.Verify(oldPwd, u.PasswordHash)) return false;
        u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPwd);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId, CancellationToken ct = default)
    {
        var u = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct);
        return u is null ? null : new UserDto(u.Id, u.Email, u.Username, u.FullName);
    }
    public async Task<UserDto?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        var u = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username, ct);
        return u is null ? null : new UserDto(u.Id, u.Email, u.Username, u.FullName);
    }
}
