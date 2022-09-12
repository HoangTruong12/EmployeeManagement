using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Employee.Modal.Dto;
using Employee.Modal.Entities;

namespace Employee.WebAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var account = (LoginDto)context.HttpContext.Items["User"];
            var account = context.HttpContext.Items["User"];
            if (account == null)
            {
                // not logged in
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
