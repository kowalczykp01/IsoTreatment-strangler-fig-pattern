namespace Application.Exceptions;

public sealed class EmailAlreadyUsedException(string email) : Exception($"Email {email} is already in use")
{
    public string Email { get; } = email;
}
