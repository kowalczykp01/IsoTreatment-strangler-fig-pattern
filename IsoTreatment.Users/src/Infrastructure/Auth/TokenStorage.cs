using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

public sealed class TokenStorage(IHttpContextAccessor httpContextAccessor) : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public JwtDto? Get()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return null;
        }
        if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
        {
            return jwt as JwtDto;
        }
        return null;
    }

    public void Set(JwtDto jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);
}
