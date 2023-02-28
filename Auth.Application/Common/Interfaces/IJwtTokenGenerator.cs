using Auth.Domain.Entities;

namespace Auth.Application.Interface;
public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
