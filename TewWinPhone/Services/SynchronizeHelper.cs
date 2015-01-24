using System;
using System.Collections.Generic;
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

        //public const string Uri = "http://localhost:8081/";
        public const string Uri = "http://yu4e4ko.somee.com/TewCloud/";

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

        public async Task<ResponseModel> GetUserWords(UserEntity user)
        {
            if (user == null)
            {
                throw new Exception("User is null");
            }

            var uri = Uri + SynchronizeController + "?userName=" + user.Email;

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

        //public ResponseModel CreatWordJsonModelAndSend(EnRuWord enRuWords, User user)
        //{
        //    var cloudModel = new WordsCloudModel { UserName = user.Email };

        //    var viewModel = new WordJsonModel
        //    {
        //        English = enRuWords.EnglishWord.EnWord,
        //        Russian = enRuWords.RussianWord.RuWord,
        //        Level = enRuWords.WordLevel,
        //        Example = enRuWords.Example,
        //        IsDeleted = enRuWords.IsDeleted,
        //        UpdateDate = enRuWords.UpdateDate
        //    };

        //    cloudModel.Words.Add(viewModel);

        //    var result = SendRequest(cloudModel);

        //    return result;
        //}
    }
}
