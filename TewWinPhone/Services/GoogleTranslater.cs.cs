using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TewWinPhone.Entities;

namespace TewWinPhone.Services
{
    internal class GoogleTranslater
    {
        private const string TranslateUri =
          "https://translate.google.by/translate_a/single?client=t&sl=en&tl=ru&hl=ru&dt=bd&dt=ex&dt=ld&dt=md&dt=qc&dt=rw&dt=rm&dt=ss&dt=t&dt=at&dt=sw&ie=UTF-8&oe=UTF-8&otf=2&ssel=0&tsel=0&q=";

        public async Task<TranslateWithExamples> GetTranslate(string word)
        {
            //word = HttpUtility.UrlEncode(word);
            string uri = TranslateUri + word;
            string responseString = string.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

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
                //throw new Exception(ex.Message, ex);
            }

            string[] translates = ParseGoogleResponse(responseString);
            string context = GetContext(responseString);

            var translateContext = new TranslateWithExamples
            {
                Example = context,
                Translates = translates
            };

            return translateContext;
        }

      
        #region privates

    
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
                            return new[] { tmpData };
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
            var mainTranslate = GetFirst(response);

            tmpData = string.Format("{0},{1}", mainTranslate, tmpData);
            string[] resultArray = tmpData.Split(',');

            resultArray = resultArray.Distinct().ToArray();

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