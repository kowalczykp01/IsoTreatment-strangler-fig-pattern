using Application.DTOs;

namespace Application.Abstractions;

public interface IAuthenticator
{
    JwtDto CreateToken(int userId);
}
