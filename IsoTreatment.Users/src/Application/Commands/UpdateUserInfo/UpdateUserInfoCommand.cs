using Application.Abstractions;

namespace Application.Commands.UpdateUserInfo;

public sealed record UpdateUserInfoCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    int Weight
) : ICommand<UpdateUserInfoCommandResult>;
