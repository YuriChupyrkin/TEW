using System.Web;
using System.Web.Http;
using TewLauncher.Services;

namespace TewCloud.Controllers
{
  public class ClientFilesInfoController : ApiController
  {
    public IHttpActionResult GetFilesInfo()
    {
      var clientDir = HttpContext.Current.Server.MapPath("/client");
      var filesInfo = new FileInfoChecker(clientDir).GetFilesInfo();
      return Json(filesInfo);
    }
  }
}