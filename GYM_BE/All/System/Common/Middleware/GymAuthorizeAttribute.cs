using API;
using GYM_BE.Core.Extentions;
using GYM_BE.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace GYM_BE.All.System.Common.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GymAuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var sid = context.HttpContext.Items["sid"];
            if (sid == null)
                context.Result = new JsonResult(new { message = "Gym Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
