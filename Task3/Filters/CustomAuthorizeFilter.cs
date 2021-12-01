using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Task3.Services;

namespace Task3.Filters
{
    public class CustomAuthorizeFilter: AuthorizeAttribute
    {
        private readonly string[] allowedRoles;
        public CustomAuthorizeFilter(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var UserName = Convert.ToString(httpContext.Session["UserName"]);
            if(!string.IsNullOrEmpty(UserName))
            {
                using (var db = new DBModel())
                {
                    var userRole = (from u in db.Users
                                    join r in db.Roles on u.RoleId equals r.RoleId
                                    where u.UserName == UserName
                                    select new
                                    {
                                        r.Name
                                    }).FirstOrDefault();
                    foreach (var role in allowedRoles)
                    {
                        if (role == userRole.Name) return true;
                    }
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Unauthorized" }
                });
        }
    }
}