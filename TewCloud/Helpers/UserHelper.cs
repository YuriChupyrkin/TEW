using Domain.Entities;
using Domain.RepositoryFactories;
using Domain.UnitOfWork;
using EnglishLearnBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TewCloud.Helpers
{
  public class UserHelper
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IUnitOfWork _unitOfWork;

    public UserHelper(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
      _unitOfWork = (IUnitOfWork)repositoryFactory;
    }

    public User GetUser(string userName)
    {
      var user = _repositoryFactory.UserRepository
        .All().FirstOrDefault(r => r.Email == userName);

      return user;
    }

    public int GetUserId(string userName)
    {
      var user = GetUser(userName);

      if(user == null)
      {
        throw new Exception("user is null");
      }

      return user.Id;
    }

    public int GetWordCount(string userName)
    {
      return _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Count(r => r.User.Email == userName);
    }

    public WordsCloudModel GetUserWords(UserUpdateDateModel updateModel)
    {
      var user = GetUser(updateModel.UserName);

      if (user == null)
      {
        throw new Exception("User not found");
      }

      var enRuWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
          .Where(r => r.UserId == user.Id);

      var wordsCloudModel = CreateWordsCloudModel(updateModel.UserName, enRuWords);

      return wordsCloudModel;
    }

    public WordsCloudModel CreateWordsCloudModel(string userName, IEnumerable<EnRuWord> enRuWords)
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
          UpdateDate = word.UpdateDate,
          AnswerCount = word.AnswerCount,
          FailAnswerCount = word.FailAnswerCount
        };

        words.Add(viewModel);
      }

      wordsCloudModel.Words = words;
      wordsCloudModel.TotalWords = GetWordCount(userName);

      return wordsCloudModel;
    }
  }
}