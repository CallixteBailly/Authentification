using FluentValidation;

namespace Auth.Application.Queries.Login;
public class VerifyTokenQueryValidator : AbstractValidator<LoginQuery>
{
    public VerifyTokenQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$").NotEmpty();
    }
}
