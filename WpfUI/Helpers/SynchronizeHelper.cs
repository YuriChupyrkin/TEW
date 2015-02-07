using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Domain.Entities;
using EnglishLearnBLL.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;

namespace WpfUI.Helpers
{
    internal sealed class SynchronizeHelper
    {
        private const string KickController = "api/Values";
        private const string SynchronizeController = "api/Synchronize";
        private const string CheckUpdateController = "api/CheckUpdate";

        //public const string Uri = "http://localhost:8081/";
        public const string Uri = "http://yu4e4ko.somee.com/TewCloud/";

        public bool IsServerOnline()
        {
            const int checkLength = 7;
            var uri = Uri + KickController + "?length=" + checkLength;

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

        public CheckUpdateModel GetUpdates(string userName)
        {
            var uri = Uri + CheckUpdateController + "?userName=" + userName;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            CheckUpdateModel result;

            try
            {
                WebResponse response = httpWebRequest.GetResponse();
                var responseStream = response.GetResponseStream();

                using (var stream = new StreamReader(responseStream))
                {
                    var responseString = stream.ReadToEnd();
                    result = JsonConvert.DeserializeObject<CheckUpdateModel>(responseString);
                }
            }
            catch (Exception ex)
            {
                return new CheckUpdateModel { IsError = true, ErrorMessage = ex.Message };
            }

            return result;
        }

        public ResponseModel GetUserWords(UserUpdateDateModel updateModel)
        {
            var uri = Uri + SynchronizeController + "?UserName=" + updateModel.UserName + "&UpdateDate=" + updateModel.UpdateDate;

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
                return new ResponseModel { IsError = true, ErrorMessage = ex.Message };
            }

            if (result == null)
            {
                return new ResponseModel
                {
                    IsError = true,
                    ErrorMessage = "response is null???"
                };
            }

            return new ResponseModel { IsError = false, ErrorMessage = string.Empty, WordsCloudModel = result };
        }

        public ResponseModel SendRequest(WordsCloudModel cloudModel)
        {
            var uri = Uri + SynchronizeController;

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

        public ResponseModel CreatWordJsonModelAndSend(EnRuWord enRuWord, User user)
        {
            var cloudModel = new WordsCloudModel { UserName = user.Email };

            var viewModel = new WordJsonModel
            {
                English = enRuWord.EnglishWord.EnWord,
                Russian = enRuWord.RussianWord.RuWord,
                Level = enRuWord.WordLevel,
                Example = enRuWord.Example,
                IsDeleted = enRuWord.IsDeleted,
                UpdateDate = enRuWord.UpdateDate
            };

            cloudModel.Words.Add(viewModel);

            var result = SendRequest(cloudModel);

            return result;
        }

        public void SendWordInBackGround(EnRuWord enRuWords, User user)
        {
            Task.Run(() => CreatWordJsonModelAndSend(enRuWords, user));
        }
    }
}
