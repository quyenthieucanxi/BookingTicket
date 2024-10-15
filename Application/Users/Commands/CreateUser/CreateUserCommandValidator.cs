using FluentValidation;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6).MaximumLength(20).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
    }
}