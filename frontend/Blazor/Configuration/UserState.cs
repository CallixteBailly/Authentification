namespace Blazor.Configuration;

public class UserState
{   
    public bool IsAuthenticated { get; set; }
    public string? Token { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public void Reset()
    {
        IsAuthenticated = false; Token = null; FirstName = null; LastName = null;
    }
}
