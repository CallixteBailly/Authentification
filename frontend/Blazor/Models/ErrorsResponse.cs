using System.Text;

namespace Blazor.Models;
public class ErrorsResponse
{
	public string? Type { get; set; }
	public string? Title { get; set; }
	public int Status { get; set; }
	public string? TraceId { get; set; }
	public Dictionary<string, string[]>? Errors { get; set; }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (Errors != null)
        {
            foreach (var error in Errors)
            {
                sb.AppendLine($"{string.Join(", ", error.Value)}");
            }
        }

        return sb.ToString();
    }
}
