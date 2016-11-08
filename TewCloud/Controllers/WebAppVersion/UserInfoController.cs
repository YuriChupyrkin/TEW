using System.Web.Http;
using System.Web.Security;
using TewCloud.Providers;

namespace TewCloud.Controllers.WebAppVersion
{
    public class UserInfoController : ApiController
    {
        public IHttpActionResult GetCurrentUserInfo()
        {
            var userEmail = User.Identity.Name;

            var user = ((TewMembershipProvider)Membership.Provider).GetUserByEmail(userEmail);
            return Json(user);
        }
    }
}
