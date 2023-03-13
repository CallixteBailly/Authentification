using FluentValidation;

namespace Auth.API.Application.Commands;
public class RigisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RigisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$").NotEmpty();
    }
}