using GeoQuest.Enums;
using System.Security.Claims;

namespace GeoQuest.Middlewares.UserContext
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserContext UserContext)
        {

            var cookie = context.Request.Cookies["GeoQuestCookie"];

            if (cookie is not null)
            {

                // Extract user information from cookies, headers, or any other source and populate UserContext object

                UserContext.Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value != null ? int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!) : -1;
                UserContext.Email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                UserContext.Role = (UserRole)mapUserRole(context.User.FindFirst(ClaimTypes.Role)?.Value);
            }

            await _next(context);
        }



        private int mapUserRole(string? role)
        {
            switch (role)
            {
                case "Teacher":
                    return 0;
                case "Student":
                    return 1;
                default:
                    throw new Exception("Invalid user role in cookie");
            }
        }
    }
}
