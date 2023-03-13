using Auth.Domain.Entities;

namespace Auth.Common
{
    public record AuthenticationResult(
        User User,
		Token Token);
}