namespace Auth.Contracts.Authentication
{
    public record AuthenticationResponse(
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        Token Token);
        
    public class Token
    {
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
	}
}