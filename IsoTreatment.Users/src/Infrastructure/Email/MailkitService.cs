using Application.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Email;

public sealed class MailkitService(IOptions<EmailOptions> options) : IEmailSender
{
    private readonly EmailOptions _options = options.Value;

    public void SendEmailConfirmationMail(string userEmail, string emailConfirmationToken)
    {
        var confirmationLink = new Uri(_options.ConfirmEmailUrl + emailConfirmationToken);
        var body =
            "<html><body><h1>Witamy w IsoSupport!</h1>"
            + $"<p>Kliknij <a href='{confirmationLink}'>tutaj</a> aby potwierdzić swój adres email.</p></body></html>";

        Send(userEmail, "Potwierdź swój adres email", body);
    }

    public void SendResetPasswordMail(string userEmail, string resetPasswordToken)
    {
        var resetLink = new Uri(_options.ResetPasswordUrl + resetPasswordToken);
        var body =
            "<html><body><h1>Zmień hasło swojego konta w IsoSupport</h1>"
            + $"<p>Kliknij <a href='{resetLink}'>tutaj</a> aby zmienić swoje hasło.</p></body></html>";

        Send(userEmail, "Zmiana hasła", body);
    }

    private void Send(string userEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_options.Username));
        email.To.Add(MailboxAddress.Parse(userEmail));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_options.Username, _options.Password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
