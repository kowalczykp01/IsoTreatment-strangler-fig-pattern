using Application.Abstractions;
using Application.Exceptions;
using Domain;
using Domain.UnitOfWork;

namespace Application.Commands.SignUp;

public sealed class SignUpCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    IEmailSender emailSender
) : ICommandHandler<SignUpCommand>
{
    public async Task HandleAsync(SignUpCommand command)
    {
        if (await unitOfWork.UserRepository.GetByEmailAsync(command.Email) is not null)
        {
            throw new EmailAlreadyUsedException(command.Email);
        }

        var securedPassword = passwordManager.Secure(command.Password);

        var user = User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            securedPassword,
            false,
            command.Weight,
            command.ClimaxDoseInMiligramsPerKilogramOfBodyWeight,
            command.DailyDose,
            command.MedicationStartDate
        );

        await unitOfWork.UserRepository.CreateAsync(user);

        var emailConfirmationToken = authenticator.CreateEmailToken(user.Email);
        emailSender.SendEmailConfirmationMail(user.Email, emailConfirmationToken);

        await unitOfWork.SaveChangesAsync();
    }
}
