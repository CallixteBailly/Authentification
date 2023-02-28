using Auth.Common;
using ErrorOr;
using MediatR;

namespace Auth.API.Application.Queries;
public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;