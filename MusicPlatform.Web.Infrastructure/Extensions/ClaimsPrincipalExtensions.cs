namespace MusicPlatform.Web.Infrastructure.Extensions
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAuthenticated(this ClaimsPrincipal user)
        {
            return user.Identity?
                .IsAuthenticated ?? false;
        }

        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
