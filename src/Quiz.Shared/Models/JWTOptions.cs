namespace Quiz.Shared.Models;
public class JWTOptions
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int Expiry { get; set; }

    public string Key { get; set; } = string.Empty;
}