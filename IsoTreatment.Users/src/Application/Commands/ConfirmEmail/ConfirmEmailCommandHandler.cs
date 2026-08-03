using Application.Abstractions;
using Application.Exceptions;
using Application.Services;
using Domain.UnitOfWork;

namespace Application.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
    : ICommandHandler<ConfirmEmailCommand>
{
    public async Task HandleAsync(ConfirmEmailCommand command)
    {
        var email = tokenService.GetEmailFromToken(command.Token);

        var user =
            await unitOfWork.UserRepository.GetByEmailAsync(email ?? string.Empty)
            ?? throw new UserNotFoundException();

        if (user.EmailConfirmed)
        {
            throw new EmailAlreadyConfirmedException();
        }

        user.ConfirmEmail();

        await unitOfWork.SaveChangesAsync();
    }
}
