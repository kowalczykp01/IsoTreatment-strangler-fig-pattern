using Application.Abstractions;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

public sealed class PasswordManager(IPasswordHasher<User> passwordHasher) : IPasswordManager
{
    public string Secure(string password) => passwordHasher.HashPassword(default, password);

    public bool Validate(string password, string securedPassword) =>
        passwordHasher.VerifyHashedPassword(default, securedPassword, password)
        == PasswordVerificationResult.Success;
}
