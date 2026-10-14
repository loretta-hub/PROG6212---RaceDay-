using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.API.Services
{
    public class SessionAuthAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string? _requiredRole;

        public SessionAuthAttribute()
        {
        }

        public SessionAuthAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;

            if (session.GetInt32("UserID") == null)
            {
                context.Result = new UnauthorizedObjectResult(
                    "Please log in first.");
                return;
            }

            if (_requiredRole != null &&
                session.GetString("Role") != _requiredRole)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}