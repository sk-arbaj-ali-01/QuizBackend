using Microsoft.AspNetCore.Http;

namespace Quiz.Shared.Models;

public class CookieOptions
{
    public DateTimeOffset Expires { get; set; } = DateTimeOffset.UtcNow.AddDays(7);

    public bool HttpOnly { get; set; } = true;

    public bool Secure { get; set; } = true;

    public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;

    public bool IsEssential { get; set; } = true;
}