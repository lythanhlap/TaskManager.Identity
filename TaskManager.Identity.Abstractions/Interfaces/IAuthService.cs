using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Identity.Abstractions;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(LoginRequest req, CancellationToken ct = default); // Identifier = username hoặc email
    Task<bool> ChangePasswordAsync(string userId, string oldPwd, string newPwd, CancellationToken ct = default);
    Task<UserDto?> GetUserByIdAsync(string userId, CancellationToken ct = default);
    Task<UserDto?> FindByUsernameAsync(string username, CancellationToken ct = default);
}