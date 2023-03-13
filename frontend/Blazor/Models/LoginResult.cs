namespace Blazor.Models;

public class LoginResult
{
	public Guid UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public Token? Token { get; set; }
	public ErrorsResponse? Errors { get; set; }
}
public class Token
{
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime Expiration { get; set; }
}
