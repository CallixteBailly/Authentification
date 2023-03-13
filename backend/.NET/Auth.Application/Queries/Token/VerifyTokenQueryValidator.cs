using FluentValidation;

namespace Auth.Application.Queries.VerifyToken;
public class VerifyTokenQueryValidator : AbstractValidator<VerifyTokenQuery>
{
    public VerifyTokenQueryValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}
