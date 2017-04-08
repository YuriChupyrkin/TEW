using System.Web.Http.Controllers;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;
using System.Net.Http;
using System.Net;
using WebSite.Auth;

namespace TewCloud.FIlters
{
  public class UserActivityFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      var authorizationHeader = actionContext.Request.Headers.Authorization;

      if (authorizationHeader != null)
      {
        var scheme = authorizationHeader.Scheme;

        if (scheme.Contains("|"))
        {
          var splitted = scheme.Split('|');
          var userId = splitted[0];
          var uniqueId = splitted[1];

          if (UserProvider.GetUserUniqueId(userId) == uniqueId)
          {
            // success
            base.OnActionExecuting(actionContext);
            return;
          }
        }

      }

      actionContext.Response = actionContext.Request
        .CreateErrorResponse(HttpStatusCode.Forbidden, "Forbidden!");
    }
  }
}