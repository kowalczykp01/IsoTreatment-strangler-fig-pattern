namespace Application.Abstractions;

public interface IEmailSender
{
    void SendEmailConfirmationMail(string userEmail, string emailConfirmationToken);
    void SendResetPasswordMail(string userEmail, string resetPasswordToken);
}
