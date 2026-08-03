using Application.Abstractions;
using Application.Exceptions;
using Domain.UnitOfWork;

namespace Application.Commands.SignIn;

public sealed class SignInCommandHandler(
    IUnitOfWork unitOfWork,
    ITokenStorage tokenStorage,
    IPasswordManager passwordManager,
    IAuthenticator authenticator
) : ICommandHandler<SignInCommand>
{
    public async Task HandleAsync(SignInCommand command)
    {
        var user = await unitOfWork.UserRepository.GetByEmailAsync(command.Email);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!user.EmailConfirmed)
        {
            throw new EmailNotConfirmedException();
        }

        if (!passwordManager.Validate(command.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        var jwt = authenticator.CreateToken(user.Id);
        tokenStorage.Set(jwt);
    }
}
