using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NAudio.Wave;
using WpfUI.Entities;

namespace WpfUI.Helpers
{
  internal class GoogleTranslater
  {
    private const string TranslateUri =
      "https://translate.google.by/translate_a/single?client=t&sl=en&tl=ru&hl=ru&dt=bd&dt=ex&dt=ld&dt=md&dt=qc&dt=rw&dt=rm&dt=ss&dt=t&dt=at&dt=sw&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&q=";

    private DirectSoundOut _directSoundOut;
    private AudioFileReader _audioFileReader;

    public GoogleTranslater()
    {
      _directSoundOut = new DirectSoundOut();
    }

    public async Task<TranslateWithContext> GetTranslate(string word)
    {
      word = word.Replace(" ", "%20");
      string uri = TranslateUri + word;
      string responseString = string.Empty;

      var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);

      try
      {
        WebResponse response = await httpWebRequest.GetResponseAsync();
        using (var stream = new StreamReader(response.GetResponseStream()))
        {
          responseString = stream.ReadToEnd();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }

      string[] translates = ParseGoogleResponse(responseString);
      string context = GetContext(responseString);

      var translateContext = new TranslateWithContext
      {
        Context = context,
        Translates = translates
      };

      return translateContext;
    }

    public async Task Speak(string word, string lang)
    {
      word = HttpUtility.UrlEncode(word);

      if (lang != "ru" && lang != "en")
      {
        lang = "en";
      }

      string uri = string.Format(
        "https://translate.google.by/translate_tts?ie=UTF-8&q={0}&tl={1}&total=1&idx=0&textlen=14&client=t&prev=input&sa=X",
        word, lang);

      var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);

      try
      {
        var response = (HttpWebResponse) await httpWebRequest.GetResponseAsync();
        long len = response.ContentLength;

        Stream ms = new MemoryStream();

        using (var stream = response.GetResponseStream())
        {
          var buffer = new byte[len];
          int read;
          while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
          {
            var pos = ms.Position;
            ms.Position = ms.Length;
            ms.Write(buffer, 0, read);
            ms.Position = pos;
          }
        }

        ms.Position = 0;
        var thread = new Thread(PlayWord);
        thread.Start(ms);     
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex);
      }
    }

    #region privates

    private void PlayWord(object obj)
    {
      var ms = obj as MemoryStream;
      using (WaveStream blockAlignedStream = new BlockAlignReductionStream(
          WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(ms))))
      {
        using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
        {
          waveOut.Init(blockAlignedStream);
          waveOut.Play();
          while (waveOut.PlaybackState == PlaybackState.Playing)
          {
            Thread.Sleep(100);
          }
        }
      }
      ms.Dispose();
    }

    private string[] ParseGoogleResponse(string response)
    {
      string tmpData = string.Empty;

      int bracketsCount = 0;
      for (int i = 0; i < response.Length; i++)
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
              return new[] {tmpData};
            }
            break;
          }
        }
      }

      for (int i = 0; i < tmpData.Length; i++)
      {
        if (tmpData[i] == ']')
        {
          tmpData = tmpData.Substring(0, i);
          break;
        }
      }

      tmpData = tmpData.Replace("\"", "");
      string[] resultArray = tmpData.Split(',');

      if (resultArray.Length < 2)
      {
        tmpData = GetFirst(response);
        return new[] { tmpData };
      }

      return resultArray;
    }

    private string GetFirst(string response)
    {
      string tmpData = string.Empty;

      int index = response.IndexOf("\"");

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
      string tmpData = string.Empty;

      int index = response.IndexOf(".001\",\"");
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

    #endregion
  }
}