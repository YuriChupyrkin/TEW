using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using TewWinPhone.Models;
using System.Threading.Tasks;
using TewWinPhone.Entities;

namespace TewWinPhone.Services
{
    internal sealed class SynchronizeHelper
    {
        private const string KickController = "api/Values";
        private const string SynchronizeController = "api/Synchronize";
        private const string CheckUpdateController = "api/CheckUpdate";

        //public const string Uri = "http://localhost:8081/";
        //public const string Uri = "http://yu4e4ko.somee.com/TewCloud/";
        public const string Uri = "http://tew.azurewebsites.net/";

        public async Task<bool> IsServerOnline()
        {
            const int checkLength = 7;
            var uri = Uri + KickController + "?length=" + checkLength;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            string[] result = null;
            try
            {
                WebResponse response = await httpWebRequest.GetResponseAsync();
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

        public async Task<CheckUpdateModel> GetUpdates(string userName)
        {
            var uri = Uri + CheckUpdateController + "?userName=" + userName;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            CheckUpdateModel result;

            try
            {
                WebResponse response = await httpWebRequest.GetResponseAsync();
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

        public async Task<ResponseModel> GetUserWords(UserUpdateDateModel updateModel)
        {
            var uri = Uri + SynchronizeController + "?UserName=" + updateModel.UserName + "&UpdateDate=" + updateModel.UpdateDate;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            SyncWordsModel result;

            try
            {
                WebResponse response = await httpWebRequest.GetResponseAsync();
                var responseStream = response.GetResponseStream();

                using (var stream = new StreamReader(responseStream))
                {
                    var responseString = stream.ReadToEnd();
                    result = JsonConvert.DeserializeObject<SyncWordsModel>(responseString);
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

        public async Task<ResponseModel> SendRequest(SyncWordsModel cloudModel)
        {
            var uri = Uri + SynchronizeController;

            var req = (HttpWebRequest)WebRequest.Create(uri);
            var enc = new UTF8Encoding();

            var json = JsonConvert.SerializeObject(cloudModel);

            byte[] data = enc.GetBytes(json);

            req.Method = "POST";
            //place MIME type here
            req.ContentType = "application/json; charset=utf-8";
            //req.ContentLength = data.Length;

            Stream newStream = await req.GetRequestStreamAsync();
            newStream.Write(data, 0, data.Length);
            newStream.Dispose();

            var response = await req.GetResponseAsync();

            Stream receiveStream = response.GetResponseStream();
            ResponseModel result;

            using (var streamReader = new StreamReader(receiveStream, Encoding.UTF8))
            {
                var jsonResult = streamReader.ReadToEnd();
                result = JsonConvert.DeserializeObject<ResponseModel>(jsonResult);
            }

            if (receiveStream != null)
            {
                receiveStream.Dispose();
            }

            return result;
        }

        public async Task<ResponseModel> CreatWordJsonModelAndSend(EnglishRussianWordEntity enRuWord, string userEmail)
        {
            var cloudModel = new SyncWordsModel { UserName = userEmail };

            var viewModel = new WordModel
            {
                English = enRuWord.English,
                Russian = enRuWord.Russian,
                Level = enRuWord.WordLevel,
                Example = enRuWord.ExampleOfUse,
                IsDeleted = enRuWord.IsDeleted,
                UpdateDate = enRuWord.UpdateDate
            };

            cloudModel.Words.Add(viewModel);

            var result = await SendRequest(cloudModel);

            return result;
        }

        public void SendWordInBackGround(EnglishRussianWordEntity enRuWord, string userEmail)
        {
            Task.Run(() => CreatWordJsonModelAndSend(enRuWord, userEmail));
        }
    }
}
