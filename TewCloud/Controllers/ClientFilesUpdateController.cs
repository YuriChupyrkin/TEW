using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using NLog;

namespace TewCloud.Controllers
{
  public class ClientFilesUpdateController : ApiController
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public HttpResponseMessage GetFilesInfo(string fileName)
    {
      try
      {
        var clientDir = HttpContext.Current.Server.MapPath("/TewCloud/client");

        var clientFile = clientDir + "/" + fileName;

        var result = new HttpResponseMessage(HttpStatusCode.OK);
        var stream = new FileStream(clientFile, FileMode.Open);
        result.Content = new StreamContent(stream);
        result.Content.Headers.ContentType =
          new MediaTypeHeaderValue("application/octet-stream");
        return result;
      }
      catch (Exception ex)
      {
        Logger.Error(string.Format("ClientFilesUpdateController error: {0}", ex.Message));
      }

      return new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }
  }
}