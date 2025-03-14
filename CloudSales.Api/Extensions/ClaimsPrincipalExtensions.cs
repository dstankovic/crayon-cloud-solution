using System.Security.Claims;

namespace CloudSales.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetIntValueFromClaims(this ClaimsPrincipal user, string claimName)
    {
        var claims = (ClaimsIdentity?)user.Identity;
        var claim = claims?.FindFirst(claimName);

        return int.TryParse(claim?.Value, out var claimValue) ? claimValue : default(int?);
    }
}
