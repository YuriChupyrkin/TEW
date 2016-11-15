using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TewCloud.Controllers.WebAppVersion
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
