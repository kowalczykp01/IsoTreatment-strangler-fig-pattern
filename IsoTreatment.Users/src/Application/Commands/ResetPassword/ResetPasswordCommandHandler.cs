using Application.Abstractions;
using Application.Exceptions;
using Application.Services;
using Domain.UnitOfWork;

namespace Application.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IPasswordManager passwordManager
) : ICommandHandler<ResetPasswordCommand>
{
    public async Task HandleAsync(ResetPasswordCommand command)
    {
        var email = tokenService.GetEmailFromToken(command.Token);

        var user =
            await unitOfWork.UserRepository.GetByEmailAsync(email ?? string.Empty)
            ?? throw new UserNotFoundException();

        if (user.ResetPasswordToken != command.Token)
        {
            throw new ResetPasswordFailedException();
        }

        var securedPassword = passwordManager.Secure(command.NewPassword);
        user.ResetPassword(securedPassword);

        await unitOfWork.SaveChangesAsync();
    }
}
