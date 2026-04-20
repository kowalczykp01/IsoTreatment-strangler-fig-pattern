using Application.DTOs;

namespace Application.Abstractions;

public interface ITokenStorage
{
    void Set(JwtDto jwt);
    JwtDto? Get();
}
