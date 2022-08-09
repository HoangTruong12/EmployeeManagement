using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(token))
            {
                // not logged in
                //tra ve trang login
                context.Result = new RedirectToActionResult("Index", "Auth", new {  }, false);
            }
        }
    }
}
