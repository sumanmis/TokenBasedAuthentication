using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Security.Claims;

namespace AuthenticationAPP
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        //protected override bool IsAuthorized(HttpActionContext actionContext)
        //{
        //    //return base.IsAuthorized(actionContext);
        //    var princial = actionContext.RequestContext.Principal as ClaimsPrincipal;

           
        //}
    }
}