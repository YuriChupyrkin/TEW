using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace TewCloud.Controllers
{
  public class ClientFilesUpdateController : ApiController
  {
    public HttpResponseMessage GetFilesInfo(string fileName)
    {
      var clientDir = HttpContext.Current.Server.MapPath("/client");

      var clientFile = clientDir + "/" + fileName;

      var result = new HttpResponseMessage(HttpStatusCode.OK);
      var stream = new FileStream(clientFile, FileMode.Open);
      result.Content = new StreamContent(stream);
      result.Content.Headers.ContentType =
          new MediaTypeHeaderValue("application/octet-stream");
      return result;
    }
  }
}