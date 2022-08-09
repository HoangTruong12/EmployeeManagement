using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Filter
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = filterContext.HttpContext.Request.Cookies["AccessToken"];

            if (token != null)
                filterContext.HttpContext.Request.Headers.Add("Authorization", $"Bearer {token}");

            base.OnActionExecuting(filterContext);
        }
    }
}
