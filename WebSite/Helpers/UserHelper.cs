﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using WebSite.Models;
using Domain.RepositoryFactories.Models;

namespace WebSite.Helpers
{
  public class UserHelper
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public UserHelper(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
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

    public WordsFullModel GetUserWords(GetUserWordsModel getUserWordsModel)
    {
      var enRuWordsQueryable = _repositoryFactory.EnRuWordsRepository.AllEnRuWordsQueryable();
      var orderedQuery = SortWords(
        enRuWordsQueryable,
        getUserWordsModel.SortKey,
        getUserWordsModel.SortAsc);

      var enRuWords = orderedQuery.Where(r => r.UserId == getUserWordsModel.UserId).AsEnumerable();
      var totalWordsCount = enRuWords.Count();

      enRuWords = enRuWords.Skip(getUserWordsModel.CurrentWordsCount).Take(getUserWordsModel.WordsPerPage);

      // user name is not necessary
      var wordsCloudModel = CreateWordsCloudModel(string.Empty, enRuWords, totalWordsCount);
      return wordsCloudModel;
    }

    private IOrderedQueryable<EnRuWord> SortWords(
      IQueryable<EnRuWord> unsortWords,
      string key,
      bool asc)
    {
      switch (key)
      {
        case "english":
          return asc ? unsortWords.OrderBy(r => r.EnglishWord.EnWord) :
            unsortWords.OrderByDescending(r => r.EnglishWord.EnWord);
        case "russian":
          return asc ? unsortWords.OrderBy(r => r.RussianWord.RuWord) :
            unsortWords.OrderByDescending(r => r.RussianWord.RuWord);
        case "level":
          return asc ? unsortWords.OrderBy(r => r.WordLevel) :
            unsortWords.OrderByDescending(r => r.WordLevel);
        case "answer":
          return asc ? unsortWords.OrderBy(r => r.AnswerCount) :
            unsortWords.OrderByDescending(r => r.AnswerCount);
        default:
          return asc ? unsortWords.OrderBy(r => r.FailAnswerCount) :
            unsortWords.OrderByDescending(r => r.FailAnswerCount);
      }
    }

    public WordsFullModel CreateWordsCloudModel(
      string userName,
      IEnumerable<EnRuWord> enRuWords,
      int totalWordsCount)
    {
      var wordsCloudModel = new WordsFullModel
      {
        UserName = userName
      };

      var words = new List<SimpleWordModel>();

      foreach (var word in enRuWords)
      {
        var viewModel = new SimpleWordModel
        {
          English = word.EnglishWord.EnWord,
          Russian = word.RussianWord.RuWord,
          Level = word.WordLevel,
          Example = word.Example,
          UpdateDate = word.UpdateDate,
          AnswerCount = word.AnswerCount,
          FailAnswerCount = word.FailAnswerCount,
          Id = word.Id
        };

        words.Add(viewModel);
      }

      wordsCloudModel.Words = words;
      wordsCloudModel.TotalWords = totalWordsCount == 0 ?GetWordCount(userName) : totalWordsCount;

      return wordsCloudModel;
    }
  }
}