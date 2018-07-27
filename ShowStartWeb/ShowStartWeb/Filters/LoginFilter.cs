using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShowStartWeb.Filters
{
    public class LoginFilter: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //User a = filterContext.HttpContext.User;
            if (filterContext.HttpContext.Session != null)
            {

            }
            else
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }

            //filterContext.HttpContext.Response.Write("123123");
        }
    }
}