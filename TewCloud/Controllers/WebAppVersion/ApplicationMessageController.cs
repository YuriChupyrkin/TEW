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
				var message = "Tew app...";

				if (Common.GlobalConfiguration.IsDevelopmentEnvironment)
				{
                message += @" Dev environment! 12334x rcr wrcwr cwr wrxcwr wrdcwr wrc";
                message += @" Dev environment!";
            }

				return message;
			}
    }
}
