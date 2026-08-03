using Application.Abstractions;

namespace Application.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(string Token) : ICommand;
