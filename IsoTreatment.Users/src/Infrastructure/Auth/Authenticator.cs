using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Application.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public sealed class Authenticator(
    IOptions<AuthOptions> options
) : IAuthenticator
{
    public JwtDto CreateToken(int userId)
    {
        var issuer = options.Value.Issuer;
        var audience = options.Value.Audience;
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };

        var expires = DateTime.Now.Add(options.Value.Expiry ?? TimeSpan.FromHours(1));

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
            SecurityAlgorithms.HmacSha256
        );

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);

        return new() { AccessToken = token };
    }
}
