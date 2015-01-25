using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EnglishLearnBLL.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TewCloud.Helpers;

namespace TewCloud.Controllers
{
    public class CheckUpdateController : ApiController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SyncHelper _syncHelper;

        public CheckUpdateController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _unitOfWork = (IUnitOfWork)repositoryFactory;
            _syncHelper = new SyncHelper(repositoryFactory);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUpdates(string userName)
        {
            try
            {
                var updateModel = _syncHelper.CheckUpdates(userName);

                return Json(updateModel);
            }
            catch (Exception ex)
            {
                return Json(new CheckUpdateModel { IsError = true, ErrorMessage = ex.Message });
            }
        }
    }
}
