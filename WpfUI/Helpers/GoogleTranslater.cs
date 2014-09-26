using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WpfUI.Entities;

namespace WpfUI.Helpers
{
  internal class GoogleTranslater
  {
    private const string Uri =
      "https://translate.google.by/translate_a/single?client=t&sl=en&tl=ru&hl=ru&dt=bd&dt=ex&dt=ld&dt=md&dt=qc&dt=rw&dt=rm&dt=ss&dt=t&dt=at&dt=sw&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&q=";

    public async Task<TranslateWithContext> GetTranslate(string word)
    {
      word = word.Replace(" ", "%20");
      string uri = Uri + word;
      var responseString = string.Empty;

      var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

      try
      {
        var response = await httpWebRequest.GetResponseAsync();
        using (var stream = new StreamReader(response.GetResponseStream()))
        {
          responseString = stream.ReadToEnd();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }

      var translates = ParseGoogleResponse(responseString);
      var context = GetContext(responseString);

      var translateContext = new TranslateWithContext
      {
        Context = context,
        Translates = translates
      };

      return translateContext;
    }

    private string[] ParseGoogleResponse(string response)
    {
      var tmpData = string.Empty;

      var bracketsCount = 0;
      for (var i = 0; i < response.Length; i++)
      {
        if (response[i] == '[')
        {
          bracketsCount++;
          if (bracketsCount == 7)
          {
            tmpData = response.Substring(i + 1);
            if (tmpData[0] != '\"')
            {
              tmpData = GetFirst(response);
              return new[] { tmpData };
            }
            break;
          }
        }
      }

      for (var i = 0; i < tmpData.Length; i++)
      {
        if (tmpData[i] == ']')
        {
          tmpData = tmpData.Substring(0, i);
          break;
        }
      }

      tmpData = tmpData.Replace("\"", "");
      var resultArray = tmpData.Split(',');

      return resultArray;
    }

    private string GetFirst(string response)
    {
      var tmpData = string.Empty;

      var index = response.IndexOf("\"");

      if (index == -1)
      {
        return tmpData;
      }

      tmpData = response.Substring(index + 1);
      index = tmpData.IndexOf("\"");

      if (index == -1)
      {
        return string.Empty;
      }

      tmpData = tmpData.Substring(0, index);
      return tmpData;
    }

    private string GetContext(string response)
    {
      var tmpData = string.Empty;

      var index = response.IndexOf(".001\",\"");
      if (index == -1)
      {
        return tmpData;
      }

      tmpData = response.Substring(index + 7);
      index = tmpData.IndexOf("\"");

      if (index == -1)
      {
        return string.Empty;
      }

      tmpData = tmpData.Substring(0, index);

      return tmpData;
    }
  }
}
