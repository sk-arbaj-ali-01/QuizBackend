using Quiz.Shared.Exceptions;
using System.Security.Claims;

namespace Quiz.Shared.Helpers;
public static class HelperExtensions
{
    public static Guid GetUserIdFromClaims(this ClaimsPrincipal principal)
    {
        Claim? userClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userClaim == null)
        {
            throw new UnAuthenticatedException("You are not logged-in. Please login properly.");
        }

        string userId = userClaim.Value;
        return Guid.Parse(userId);
    }
}
