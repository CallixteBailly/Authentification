using Auth.Common;
using Auth.Domain.Entities;
using Auth.Domain.Errors;
using MediatR;
using ErrorOr;
using Auth.Application.Interface;

namespace Auth.Application.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_userRepository.GetUserByEmail(request.Email) is not User user)
            return Errors.Authentication.InvalidCredentials;

        if (!_userRepository.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return Errors.Authentication.InvalidCredentials;

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}
