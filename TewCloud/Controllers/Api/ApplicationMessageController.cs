using System.Web.Http;

namespace TewCloud.Controllers.Api
{
    public class ApplicationMessageController : ApiController
    {
        [HttpGet]
        public string GetCurrentUserInfo()
        {
            var message = "Tew app... Alfa test";

            if (Common.GlobalConfiguration.IsDevelopmentEnvironment)
            {
                message += @" Dev environment! test message. hello world, hello tew";
            }

            return message;
        }
    }
}
