namespace Application.Services;

public interface ITokenService
{
    int? GetUserIdFromToken(string token);
    string? GetEmailFromToken(string token);
}
