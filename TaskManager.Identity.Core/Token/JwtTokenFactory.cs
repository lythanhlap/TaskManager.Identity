using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Identity.Abstractions;
using TaskManager.Identity.Persistence.EFCore.Entities;

namespace TaskManager.Identity.Core.Token;

internal sealed class JwtTokenFactory
{
    private readonly Options.IdentityOptions _opt;
    public JwtTokenFactory(Options.IdentityOptions opt) => _opt = opt;

    public AuthResponse Create(ApplicationUser u)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.JwtSigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opt.JwtIssuer, audience: _opt.JwtAudience,
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.Id),
                new Claim(JwtRegisteredClaimNames.Email, u.Email),
                new Claim("name", u.FullName),
                new Claim("username", u.Username)
            },
            expires: DateTime.UtcNow.AddMinutes(_opt.JwtExpiresMinutes),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthResponse(jwt, token.ValidTo, new UserDto(u.Id, u.Email, u.Username, u.FullName));
    }
}