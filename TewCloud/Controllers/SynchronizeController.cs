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
using WebGrease.Css.Extensions;

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
        .Where(r => r.UserId == user.Id && r.IsUpdated);

      var wordsCloudModel = CreateWordsCloudModel(userName, enRuWords);

      enRuWords.ForEach(r => r.IsUpdated = false);

      return wordsCloudModel;
    }

    private WordsCloudModel CreateWordsCloudModel(string userName, IEnumerable<EnRuWord> enRuWords)
    {
      var wordsCloudModel = new WordsCloudModel
      {
        UserName = userName
      };

      var words = new List<WordViewModel>();

      foreach (var word in enRuWords)
      {
        var viewModel = new WordViewModel
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
      var userEnRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.UserId == userId);

      var enWordsFromModel = wordsModel.Words.Where(r => r.IsDeleted == false).Select(r => r.English);

      foreach (var userEnRuWord in userEnRuWords)
      {
        if (enWordsFromModel.Contains(userEnRuWord.EnglishWord.EnWord))
        {
          var wordFromModel = wordsModel.Words.FirstOrDefault(r => r.English == userEnRuWord.EnglishWord.EnWord);

          if (wordFromModel.Russian != userEnRuWord.RussianWord.RuWord || wordFromModel.Level != userEnRuWord.WordLevel
              || wordFromModel.Example != userEnRuWord.Example)
          {
            _repositoryFactory.EnRuWordsRepository
            .AddTranslate(
              wordFromModel.English,
              wordFromModel.Russian,
              wordFromModel.English,
              userId,
              wordFromModel.Level);

            _repositoryFactory.EnRuWordsRepository.ChangeUpdateStatus(userEnRuWord.Id, true);
          }
        }
      }

      var deleted = wordsModel.Words.Where(r => r.IsDeleted).Select(r => r.English);

      foreach (var deletedWord in deleted)
      {
        DeleteWord(deletedWord, userId);
      }

      //foreach (var word in wordsModel.Words)
      //{
      //  if (word.IsDeleted)
      //  {
      //    DeleteWord(word.English, userId);
      //  }
      //  else
      //  {
      //    _repositoryFactory.EnRuWordsRepository
      //      .AddTranslate(
      //        word.English,
      //        word.Russian,
      //        word.English,
      //        userId,
      //        word.Level);
      //  }
      //}
    }

    private void DeleteWord(string enWord, int userId)
    {
      _repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(enWord, userId);
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