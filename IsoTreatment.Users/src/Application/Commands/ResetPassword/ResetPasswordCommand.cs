using Application.Abstractions;

namespace Application.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Token,
    string NewPassword,
    string ConfirmNewPassword
) : ICommand;
