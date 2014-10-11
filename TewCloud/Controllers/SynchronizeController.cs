using System;
using System.Collections.Generic;
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
        .Where(r => r.UserId == user.Id && r.IsUpdated == false);

      var wordsCloudModel = CreateWordsCloudModel(userName, enRuWords);

      var updatedWord = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
        .Where(r => r.UserId == user.Id && r.IsUpdated);

      foreach (var enRuWord in updatedWord)
      {
        _repositoryFactory.EnRuWordsRepository.ChangeUpdateStatus(enRuWord.Id, false);
      }

      return wordsCloudModel;
    }

    private WordsCloudModel CreateWordsCloudModel(string userName, IEnumerable<EnRuWord> enRuWords)
    {
      var wordsCloudModel = new WordsCloudModel
      {
        UserName = userName
      };

      var words = new List<WordJsonModel>();

      foreach (var word in enRuWords)
      {
        var viewModel = new WordJsonModel
        {
          English = word.EnglishWord.EnWord,
          Russian = word.RussianWord.RuWord,
          Level = word.WordLevel,
          Example = word.Example,
          UpdateDate = word.UpdateDate
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
      var userExistWords = userEnRuWords.Select(r => r.EnglishWord.EnWord);

      var enWordsFromModel = wordsModel.Words.Where(r => r.IsDeleted == false).Select(r => r.English);

      foreach (var wordFromModel in enWordsFromModel)
      {
        if (userExistWords.Contains(wordFromModel))
        {
          var userEnRuWord = userEnRuWords.FirstOrDefault(r => r.EnglishWord.EnWord == wordFromModel);
          var modelItem = wordsModel.Words.FirstOrDefault(r => r.English == wordFromModel);

          if (modelItem.UpdateDate > userEnRuWord.UpdateDate)
          {
            _repositoryFactory.EnRuWordsRepository
              .AddTranslate(
                modelItem.English,
                modelItem.Russian,
                modelItem.Example,
                userId,
                modelItem.UpdateDate,
                modelItem.Level);

            _repositoryFactory.EnRuWordsRepository.ChangeUpdateStatus(userEnRuWord.Id, true);
          }
        }
        else
        {
          var modelItem = wordsModel.Words.FirstOrDefault(r => r.English == wordFromModel);

          _repositoryFactory.EnRuWordsRepository
              .AddTranslate(
                modelItem.English,
                modelItem.Russian,
                modelItem.Example,
                userId,
                modelItem.UpdateDate,
                modelItem.Level);

          var id = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
              .FirstOrDefault(r => r.EnglishWord.EnWord == modelItem.English)
              .Id;
          _repositoryFactory.EnRuWordsRepository.ChangeUpdateStatus(id, true);
        }
      }

      var deleted = wordsModel.Words.Where(r => r.IsDeleted).Select(r => r.English);

      foreach (var deletedWord in deleted)
      {
        DeleteWord(deletedWord, userId);
      }
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