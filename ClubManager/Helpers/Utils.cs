using System.Security.Claims;

namespace ClubManager.Helpers
{
    public class Utils
    {
        public static long GetCurrentUserId(ClaimsPrincipal currentUser)
        {
            return long.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public static string GetCurrentUsername(ClaimsPrincipal currentUser)
        {
            return currentUser.FindFirst(ClaimTypes.Name).Value;
        }
    }
}