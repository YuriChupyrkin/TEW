//using Domain.RepositoryFactories;
//using EnglishLearnBLL.Models;
//using System;
//using System.Threading.Tasks;
//using System.Web.Http;
//using TewCloud.Helpers;

//namespace TewCloud.Controllers
//{
//    public class CheckUpdateController : ApiController
//    {
//        private readonly SyncHelper _syncHelper;

//        public CheckUpdateController(IRepositoryFactory repositoryFactory)
//        {
//            _syncHelper = new SyncHelper(repositoryFactory);
//        }

//        [HttpGet]
//        public async Task<IHttpActionResult> GetUpdates(string userName)
//        {
//            try
//            {
//                var updateModel = _syncHelper.CheckUpdates(userName);

//                return Json(updateModel);
//            }
//            catch (Exception ex)
//            {
//                return Json(new CheckUpdateModel { IsError = true, ErrorMessage = ex.Message });
//            }
//        }
//    }
//}
