using Auth.Common;
using ErrorOr;
using MediatR;

namespace Auth.Application.Queries.VerifyToken;
public record VerifyTokenQuery(string Token) : IRequest<ErrorOr<bool>>;