using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TewCloud.Helpers;

namespace TewCloud.Controllers
{
    public class SynchronizeV2Controller : ApiController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SyncHelper _syncHelper;

        public SynchronizeV2Controller(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _unitOfWork = (IUnitOfWork)repositoryFactory;
            _syncHelper = new SyncHelper(repositoryFactory);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddWords([FromBody] WordsCloudModel wordsModel)
        {
            try
            {
                if(wordsModel == null)
                {
                    throw new ArgumentException("words model is null");
                }

                var userId = _syncHelper.GetUserId(wordsModel.UserName);

                foreach (var modelItem in wordsModel.Words)
                {
                    _repositoryFactory.EnRuWordsRepository
                      .AddTranslate(
                        modelItem.English,
                        modelItem.Russian,
                        modelItem.Example,
                        userId,
                        modelItem.UpdateDate,
                        modelItem.Level,
                        modelItem.IsDeleted);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };

                return Json(response);
            }

            var okResponse = new ResponseModel
            {
                IsError = false,
                ErrorMessage = string.Empty
            };
            return Json(okResponse);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetWords(UserUpdateDateModel updateModel)
        {
            WordsCloudModel cloudModel;
            try
            {
                cloudModel = _syncHelper.GetUserWords(updateModel);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    IsError = true,
                    ErrorMessage = ex.Message
                };

                return Json(response);
            }

            return Json(cloudModel);
        }

    }
}
