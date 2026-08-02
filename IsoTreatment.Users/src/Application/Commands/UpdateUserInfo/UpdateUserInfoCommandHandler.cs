using Application.Abstractions;
using Application.Exceptions;
using Domain.UnitOfWork;

namespace Application.Commands.UpdateUserInfo;

public sealed class UpdateUserInfoCommandHandler(
    IUnitOfWork unitOfWork,
    IAuthenticator authenticator,
    IEmailSender emailSender
) : ICommandHandler<UpdateUserInfoCommand, UpdateUserInfoCommandResult>
{
    public async Task<UpdateUserInfoCommandResult> HandleAsync(UpdateUserInfoCommand command)
    {
        var user =
            await unitOfWork.UserRepository.GetByIdAsync(command.Id)
            ?? throw new UserNotFoundException();

        var isEmailChanged = user.Email != command.Email;

        user.UpdateUser(
            command.FirstName,
            command.LastName,
            command.Weight,
            isEmailChanged ? command.Email : null
        );

        await unitOfWork.SaveChangesAsync();

        if (isEmailChanged)
        {
            var emailConfirmationToken = authenticator.CreateEmailToken(user.Email);
            emailSender.SendEmailConfirmationMail(user.Email, emailConfirmationToken);
        }

        return new UpdateUserInfoCommandResult(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Weight
        );
    }
}
