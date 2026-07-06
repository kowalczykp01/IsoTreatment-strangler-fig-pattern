using System.Security.Claims;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Commands.SignIn;
using Application.Commands.SignUp;
using Application.Queries.GetUserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("api/user")]
    [ApiController]
    public class UserController(ITokenStorage tokenStorage) : ControllerBase
    {
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult<GetUserInfoQueryResult>> GetUserById(
            [FromServices] IQueryHandler<GetUserInfoQuery, GetUserInfoQueryResult> handler
        )
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await handler.HandleAsync(new(userId));

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(
            [FromBody] SignInCommand command,
            [FromServices] ICommandHandler<SignInCommand> handler
        )
        {
            await handler.HandleAsync(command);
            var jwt = tokenStorage.Get();

            HttpContext.Response.Cookies.Append(
                "token",
                jwt!.AccessToken,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                }
            );

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(
            [FromBody] SignUpCommand command,
            [FromServices] ICommandHandler<SignUpCommand> handler
        )
        {
            await handler.HandleAsync(command);

            return Ok();
        }
    }
}
