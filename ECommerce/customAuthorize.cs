

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc.Filters;

namespace ECommerce;

public class customAuthorize :ActionFilterAttribute
{
    //public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
    //{
    //    throw new NotImplementedException();
    //}

    //// ActionFilterAttribute, IAuthenticationFilter
    //public void OnAuthentication(AuthenticationContext filterContext)
    //{
    //    if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserName"])))
    //    {
    //        filterContext.Result = new HttpUnauthorizedResult();
    //    }
    //}
    //public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    //{
    //    if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
    //    {
    //        //Redirecting the user to the Login View of Account Controller  
    //        filterContext.Result = new RedirectToRouteResult(
    //        new RouteValueDictionary
    //        {
    //                 { "controller", "Account" },
    //                 { "action", "Login" }
    //        });
    //    }
    //}
    //protected override bool AuthorizeCore(HttpContextBase httpContext)
    //{
    //    var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
    //    bool authorize = false;
    //    if (Roles == "Admin")
    //    {
    //        authorize = true;
    //    }
    //    if (Roles == "Supplier")
    //    {
    //        authorize = true;
    //    }
    //    return authorize;


    //}
}
