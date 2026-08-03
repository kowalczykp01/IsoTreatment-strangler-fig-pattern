using Application.Abstractions;
using Application.Exceptions;
using Domain.UnitOfWork;

namespace Application.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IUnitOfWork unitOfWork,
    IAuthenticator authenticator,
    IEmailSender emailSender
) : ICommandHandler<ForgotPasswordCommand>
{
    public async Task HandleAsync(ForgotPasswordCommand command)
    {
        var user =
            await unitOfWork.UserRepository.GetByEmailAsync(command.Email)
            ?? throw new UserNotFoundException();

        var resetPasswordToken = authenticator.CreateEmailToken(command.Email);
        user.SetResetPasswordToken(resetPasswordToken);

        await unitOfWork.SaveChangesAsync();

        emailSender.SendResetPasswordMail(user.Email, resetPasswordToken);
    }
}
