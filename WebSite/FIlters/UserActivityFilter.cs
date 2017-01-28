using System;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace TewCloud.FIlters
{
  public class UserActivityFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
      var authorizationHeader = actionContext.Request.Headers.Authorization;

      if (authorizationHeader != null)
      {
        var userIdString = authorizationHeader.Scheme;

        int userId;

        if (string.IsNullOrEmpty(userIdString) == false && int.TryParse(userIdString, out userId))
        {
          SaveActivity(userId);
        }
      }

      base.OnActionExecuting(actionContext);
    }

    private void SaveActivity(int userId)
    {
      var repositoryFactory = DependencyResolver.Current.GetService<IRepositoryFactory>();
      var user = repositoryFactory.UserRepository.Find(userId);

      if (user == null)
      {
        return;
      }

      user.ActivityLevel++;
      user.LastActivity = DateTime.Now;

      ((IUnitOfWork)repositoryFactory).Commit();
    }
  }
}