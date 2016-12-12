using System.Web.Mvc;

namespace TewCloud.Controllers
{
	public class TewController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}
	}
}