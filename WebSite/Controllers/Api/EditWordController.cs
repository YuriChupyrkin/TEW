using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSite.Helpers;

namespace WebSite.Controllers.Api
{
  public class EditWordController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly UserHelper _userHelper;

    public EditWordController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _userHelper = new UserHelper(repositoryFactory);
    }

    [HttpPost]
    public IHttpActionResult EditWord(WordsFullModel wordsModel)
    {
      if (wordsModel != null && wordsModel.Words.Any())
      {
        var word = wordsModel.Words.First();

        var enRuWord = new EnRuWord
        {
           RussianWord = new RussianWord { RuWord = word.Russian },
           Example = word.Example,
           Id = word.Id,
           FailAnswerCount = word.FailAnswerCount,
           WordLevel = word.Level,
           AnswerCount = word.AnswerCount
        };

        _repositoryFactory.EnRuWordsRepository.EditEnRuWord(enRuWord, wordsModel.UserId);

        return Ok();
      }

      return NotFound();
    }
  }
}
