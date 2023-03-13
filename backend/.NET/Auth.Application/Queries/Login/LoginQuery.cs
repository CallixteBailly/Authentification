using Auth.Common;
using ErrorOr;
using MediatR;

namespace Auth.Application.Queries.Login;
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;