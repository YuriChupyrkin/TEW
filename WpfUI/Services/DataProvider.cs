using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EnglishLearnBLL.Models;
using Newtonsoft.Json;

namespace WpfUI.Services
{
	internal class DataProvider
	{
		protected const string Uri = "http://tew.azurewebsites.net/";
		//protected const string Uri = "http://localhost/";

		protected const string SignUpController = "api/SignUp";
		protected const string SignInController = "api/SignIn";

		protected static async Task<TOutput> SendPostRequestAsync<TInput, TOutput>(TInput tInput, string controllerName)
		{
			var uri = Uri + controllerName;

			var request = (HttpWebRequest)WebRequest.Create(uri);
			var enc = new UTF8Encoding();

			var json = JsonConvert.SerializeObject(tInput);
			byte[] data = enc.GetBytes(json);

			request.Method = "POST";
			//place MIME type here
			request.ContentType = "application/json; charset=utf-8";
			request.ContentLength = data.Length;

			using (var newStream = await request.GetRequestStreamAsync())
			{
				newStream.Write(data, 0, data.Length);
			}

			var response = request.GetResponse();
			TOutput result;

			using (var responseStream = response.GetResponseStream())
			{
				using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
				{
					var jsonResult = streamReader.ReadToEnd();
					result = JsonConvert.DeserializeObject<TOutput>(jsonResult);
				}
			}

			return result;
		}

		// TODO CREATE GET REQUEST
		public ResponseModel GetUserWords(UserUpdateDateModel updateModel)
		{
			var uri = Uri + "SynchronizeController" + "?UserName=" + updateModel.UserName + "&UpdateDate=" + updateModel.UpdateDate;

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
	}
}
