using System.Web.Mvc;

namespace WebSite.Controllers
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