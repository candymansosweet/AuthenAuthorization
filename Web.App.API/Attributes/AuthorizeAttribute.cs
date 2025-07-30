using Common.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Common.Constants.RoleValue;

namespace Web.App.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public AuthorizeAttribute() { }

        public AuthorizeAttribute(params string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            // authorization
            var userId = context.HttpContext.Items[ContextItems.UserId];

            if (userId == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            List<string>? userPermissions = context.HttpContext.Items[ContextItems.Permissions] as List<string>;

            if (userPermissions == null || userPermissions.Count == 0)
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }

            if (
                _requiredPermissions != null
                && _requiredPermissions.Length > 0
                && _requiredPermissions.Any(
                        per => !userPermissions.Contains(per)
                    )
            )
            {
                context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
                return;
            }
        }
    }
}
