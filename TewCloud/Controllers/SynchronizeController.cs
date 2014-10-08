using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EnglishLearnBLL.Models;

namespace TewCloud.Controllers
{
  public class SynchronizeController : ApiController
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public SynchronizeController(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork) repositoryFactory;
    }

    [HttpPost]
    public async Task<IHttpActionResult> SetWords([FromBody] WordsCloudModel wordsModel)
    {
      try
      {
        AddUserWords(wordsModel);
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
    public async Task<IHttpActionResult> GetWords(string userName)
    {
      WordsCloudModel cloudModel;
      try
      {
        cloudModel = GetUserWords(userName);
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

    #region privates 

    // /////////////////// Get words ///////////////////

    private WordsCloudModel GetUserWords(string userName)
    {
      var user = GetUser(userName);

      if (user == null)
      {
        throw new Exception("User not found");
      }

      var enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
        .Where(r => r.UserId == user.Id);

      return CreateWordsCloudModel(userName, enRuWords);
    }

    private WordsCloudModel CreateWordsCloudModel(string userName, IEnumerable<EnRuWord> enRuWords)
    {
      var wordsCloudModel = new WordsCloudModel
      {
        UserName = userName
      };

      var words = new List<EnRuWordViewModel>();

      foreach (var word in enRuWords)
      {
        var viewModel = new EnRuWordViewModel
        {
          English = word.EnglishWord.EnWord,
          Russian = word.RussianWord.RuWord,
          Level = word.WordLevel,
          Example = word.Example
        };

        words.Add(viewModel);
      }

      wordsCloudModel.Words = words;

      return wordsCloudModel;
    }

    // /////////////////// Set words ///////////////////

    private void AddUserWords(WordsCloudModel wordsModel)
    {
      var userId = GetUserId(wordsModel.UserName);

      foreach (var word in wordsModel.Words)
      {
        _repositoryFactory.EnRuWordsRepository
          .AddTranslate(
          word.English, 
          word.Russian, 
          word.English, 
          userId, 
          word.Level);
      }

      var allWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == userId);

      var allwordsFromWordsModel = wordsModel.Words.Select(r => r.English);
      foreach (var word in allWords)
      {
        var engWord = word.EnglishWord.EnWord;
        if (allwordsFromWordsModel.Contains(engWord) == false)
        {
          _repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(engWord, userId);
        }
      }   
    }

    private int GetUserId(string userName)
    {
      var user = GetUser(userName);
      if (user == null)
      {
        CreateNewUser(userName);
      }

      user = GetUser(userName);
      if (user != null)
      {
        return user.Id;
      }

      throw new Exception("User creating error");
    }

    private void CreateNewUser(string userName)
    {
      var newUser = new User
      {
        Email = userName,
        Password = "password",
        RoleId = 2
      };

      _repositoryFactory.UserRepository.Create(newUser);
      _unitOfWork.Commit();
    }

    private User GetUser(string userName)
    {
      var user = _repositoryFactory.UserRepository
        .All().FirstOrDefault(r => r.Email == userName);
      return user;
    }

    #endregion
  }
}