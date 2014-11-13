using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Domain.Entities;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using Newtonsoft.Json;

namespace WpfUI.Helpers
{
  internal sealed class SynchronizeHelper
  {
    private const string _kickController = "api/Values";
    private const string _synchronizeController = "api/Synchronize";

    public const string Uri = "http://localhost:8081/";
    //public const string Uri = "http://yu4e4ko.somee.com/TewCloud/";

    private readonly IRepositoryFactory _repositoryFactory;

    public SynchronizeHelper(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public bool IsServerOnline()
    {
      const int checkLength = 7;
      var uri = Uri + _kickController + "?length=" + checkLength;

      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

      string[] result = null;
      try
      {
        WebResponse response = httpWebRequest.GetResponse();
        var responseStream = response.GetResponseStream();
        using (var stream = new StreamReader(responseStream))
        {
          var responseString = stream.ReadToEnd();
          result = JsonConvert.DeserializeObject<string[]>(responseString);
        }
      }
      catch (Exception ex)
      {
        return false;
      }

      if (result == null || result.Length != checkLength)
      {
        return false;
      }
      return true;
    }

    public ResponseModel SendMyWords(User user)
    {
      if (user == null)
      {
        throw new Exception("User is null");
      }

      var userWords = _repositoryFactory.EnRuWordsRepository
        .AllEnRuWords().Where(r => r.UserId == user.Id);

      if (userWords == null)
      {
        throw new Exception("User words is null");
      }

      var cloudModel = new WordsCloudModel { UserName = user.Email };

      foreach (var word in userWords)
      {
        var viewModel = new WordJsonModel
        {
          English = word.EnglishWord.EnWord,
          Russian = word.RussianWord.RuWord,
          Level = word.WordLevel,
          Example = word.Example,
          IsDeleted = word.IsDeleted,
          UpdateDate = word.UpdateDate
        };

        cloudModel.Words.Add(viewModel);
      }

      var result = SendRequest(cloudModel);

      return result;
    }

    public ResponseModel GetUserWords(User user)
    {
      if (user == null)
      {
        throw new Exception("User is null");
      }

      var uri = Uri + _synchronizeController + "?userName=" + user.Email;

      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

      WordsCloudModel result;

      try
      {
        WebResponse response = httpWebRequest.GetResponse();
        var responseStream = response.GetResponseStream();

        using (var stream = new StreamReader(responseStream))
        {
          var responseString = stream.ReadToEnd();
          result = JsonConvert.DeserializeObject<WordsCloudModel>(responseString);
        }
      }
      catch (Exception ex)
      {
        return new ResponseModel {IsError = true, ErrorMessage = ex.Message};
      }

      if (result == null)
      {
        return new ResponseModel { 
          IsError = true, 
          ErrorMessage = "response is null???" };
      }

      //AddWordsFromResponse(result);
      //RemoveIsDeletedWords(user.Id);

      return new ResponseModel { IsError = false, ErrorMessage = string.Empty, WordsCloudModel = result };
    }

    #region privates

    private void RemoveIsDeletedWords(int userId)
    {
      var deletedWords = _repositoryFactory.EnRuWordsRepository.AllEnRuWords().Where(r => r.IsDeleted && r.UserId == userId);

      foreach (var word in deletedWords)
      {
        _repositoryFactory.EnRuWordsRepository.DeleteEnRuWord(word.EnglishWord.EnWord, userId);
      }
    }

    private void AddWordsFromResponse(WordsCloudModel cloudModel)
    {
      var user = _repositoryFactory.UserRepository
        .Find(r => r.Email == cloudModel.UserName);

      if (user == null)
      {
        throw new Exception("Words for user " + cloudModel.UserName + ", but user not found");
      }

      //Parallel.ForEach(cloudModel.Words, word =>
      //  _repositoryFactory.EnRuWordsRepository.AddTranslate(
      //    word.English,
      //    word.Russian,
      //    word.Example,
      //    user.Id,
      //    word.UpdateDate,
      //    word.Level
      //    ));


      foreach (var word in cloudModel.Words)
      {
        _repositoryFactory.EnRuWordsRepository
          .AddTranslate(
          word.English, 
          word.Russian, 
          word.Example, 
          user.Id, 
          word.UpdateDate,
          word.Level);
      }
    }

    private ResponseModel SendRequest(WordsCloudModel cloudModel)
    {
      var uri = Uri + _synchronizeController;

      var req = (HttpWebRequest)WebRequest.Create(uri);
      var enc = new UTF8Encoding();

      var json = JsonConvert.SerializeObject(cloudModel);

      byte[] data = enc.GetBytes(json);

      req.Method = "POST";

      //place MIME type here
      req.ContentType = "application/json; charset=utf-8"; 
      req.ContentLength = data.Length;

      Stream newStream = req.GetRequestStream();
      newStream.Write(data, 0, data.Length);
      newStream.Close();

      var response = req.GetResponse();

      Stream receiveStream = response.GetResponseStream();
      ResponseModel result;

      using (var streamReader = new StreamReader(receiveStream, Encoding.UTF8))
      {
        var jsonResult = streamReader.ReadToEnd();
        result = JsonConvert.DeserializeObject<ResponseModel>(jsonResult);
      }

      if (receiveStream != null)
      {
        receiveStream.Close();
      }

      return result;
    }

    #endregion
  }
}
