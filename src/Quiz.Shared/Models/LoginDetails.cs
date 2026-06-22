using Quiz.Shared.Enums;

namespace Quiz.Shared.Models;
public class LoginDetails
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; } = "Bearer";

    public string Role { get; set; } = string.Empty;

    public DateTime Expires { get; set; }
}
