using System;
using System.Web;
using System.Web.Http;
using NLog;
using TewLauncher.Services;

namespace TewCloud.Controllers
{
  public class ClientFilesInfoController : ApiController
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public IHttpActionResult GetFilesInfo()
    {
      var errorMessage = string.Empty;

      try
      {
        var clientDir = HttpContext.Current.Server.MapPath("/TewCloud/client");
        var filesInfo = new FileInfoChecker(clientDir).GetFilesInfo();
        return Json(filesInfo);
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;

        Logger.Error(string.Format("ClientFilesInfoController error: {0}", errorMessage));
      }

      return Json(new { Error = errorMessage });
    }
  }
}