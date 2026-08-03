namespace Application.Commands.UpdateUserInfo;

public sealed record UpdateUserInfoCommandResult(
    string FirstName,
    string LastName,
    string Email,
    int Weight);
