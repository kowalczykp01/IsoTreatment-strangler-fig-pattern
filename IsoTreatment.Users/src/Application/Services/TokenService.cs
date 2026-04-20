using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services;

public class TokenService : ITokenService
{
    public int? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (decodedToken is not null)
        {
            var userIdClaim = decodedToken.Claims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.NameIdentifier
            );

            if (userIdClaim != null)
            {
                return Int32.Parse(userIdClaim.Value);
            }
        }

        return null;
    }
}
