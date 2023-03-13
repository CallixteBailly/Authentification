namespace Blazor.Models;

public class LoginState
{
    public bool IsAuthenticated { get; set; }
    public string? Token { get; set; }
}
