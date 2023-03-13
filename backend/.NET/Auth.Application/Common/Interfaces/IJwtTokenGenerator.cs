using Auth.Domain.Entities;

namespace Auth.Application.Interface;
public interface IJwtTokenGenerator
{
	Token GenerateToken(User user);
    bool VerifyToken(string token);
}
