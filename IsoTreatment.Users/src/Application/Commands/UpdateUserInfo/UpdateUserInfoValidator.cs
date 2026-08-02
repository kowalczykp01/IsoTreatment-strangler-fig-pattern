using FluentValidation;

namespace Application.Commands.UpdateUserInfo;

public sealed class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoCommand>
{
    public UpdateUserInfoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();

        RuleFor(x => x.LastName).NotEmpty();

        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Weight).GreaterThan(0);
    }
}
