using Auth.Common;
using ErrorOr;
using MediatR;

namespace Auth.API.Application.Commands;
public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;