using Auth.Common;
using Auth.Domain.Entities;
using Auth.Domain.Errors;
using MediatR;
using ErrorOr;
using Auth.Application.Interface;

namespace Auth.Application.Queries.VerifyToken;

public class VerifyTokenQueryHandler : IRequestHandler<VerifyTokenQuery, ErrorOr<bool>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public VerifyTokenQueryHandler(IJwtTokenGenerator jwtTokenGenerator) => _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<ErrorOr<bool>> Handle(
        VerifyTokenQuery request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (!_jwtTokenGenerator.VerifyToken(request.Token))
            return false;

        return true;
    }
}
