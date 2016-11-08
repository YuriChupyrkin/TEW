using System.Web.Mvc;

namespace TewCloud.Controllers
{
	public class TewController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[Authorize]
		public ActionResult Learning()
		{
			return View();
		}
	}
}