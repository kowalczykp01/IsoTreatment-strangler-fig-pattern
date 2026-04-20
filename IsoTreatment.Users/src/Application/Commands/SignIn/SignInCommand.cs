using Application.Abstractions;

namespace Application.Commands.SignIn;

public sealed record SignInCommand(string Email, string Password) : ICommand;
