using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TewCloud.Helpers;

namespace TewCloud.Controllers.WebAppVersion
{
    public class DeleteWordController : ApiController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly SyncHelper _syncHelper;

        public DeleteWordController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _syncHelper = new SyncHelper(repositoryFactory);
        }

        [HttpPost]
        public IHttpActionResult DeleteWord( WordsCloudModel wordsModel)
        {
            if (wordsModel != null && wordsModel.Words.Any())
            {
                var userId = _syncHelper.GetUserId(wordsModel.UserName);
                _repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(wordsModel.Words.First().English, userId);

                return Ok();
            }

            return NotFound();
        }
    }
}
