namespace Application.Services;

public interface ITokenService
{
    int? GetUserIdFromToken(string token);
}
