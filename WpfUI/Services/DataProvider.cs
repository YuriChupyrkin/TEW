using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace WpfUI.Services
{
	internal class DataProvider
	{
		protected const string Uri = "http://tew.azurewebsites.net/";
		//protected const string Uri = "http://localhost/";

		protected const string SignUpController = "api/SignUp";
		protected const string SignInController = "api/SignIn";
		protected const string WordsManagerController = "api/WordsManager";
		protected const string PickerTestsController = "api/PickerTests";
		protected const string WordsLevelUpdaterController = "api/WordsLevelUpdater";
		protected const string WordTranslaterController = "api/WordTranslater";
		protected const string WriteTestsController = "api/WriteTests";
		protected const string ChangePasswordController = "api/ChangePassword";
		protected const string ResetPasswordController = "api/ResetPassword";

		protected static async Task<TOutput> SendPostRequestAsync<TInput, TOutput>(
			TInput tInput,
			string controllerName,
			string httpMethod = null)
		{
			MainWindow.WindowMainFrame.IsEnabled = false;
			var result = await Task.Run(() => PostRequestAsync<TInput, TOutput>(tInput, controllerName, httpMethod));
			MainWindow.WindowMainFrame.IsEnabled = true;

			return result;
		}

		protected static async Task<TOutput> SendGetRequestAsync<TOutput>(
			Dictionary<string, string> queryStringParams,
			string controllerName)
		{
			MainWindow.WindowMainFrame.IsEnabled = false;
			var result = await Task.Run(() => GetRequestAsync<TOutput>(queryStringParams, controllerName));
			MainWindow.WindowMainFrame.IsEnabled = true;

			return result;
		}

		private static async Task<TOutput> PostRequestAsync<TInput, TOutput>(
			TInput tInput,
			string controllerName,
			string httpMethod = null)
		{
			var uri = Uri + controllerName;

			var request = (HttpWebRequest)WebRequest.Create(uri);
			var enc = new UTF8Encoding();

			var json = JsonConvert.SerializeObject(tInput);
			byte[] data = enc.GetBytes(json);

			request.Method = httpMethod ?? "POST";
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

					if (string.IsNullOrEmpty(jsonResult) == false)
					{
						result = JsonConvert.DeserializeObject<TOutput>(jsonResult);
					}
					else
					{
						result = JsonConvert.DeserializeObject<TOutput>(string.Empty);
					}
				}
			}

			return result;
		}

		private static async Task<TOutput> GetRequestAsync<TOutput>(
			Dictionary<string, string> queryStringParams, 
			string controllerName)
		{
			var uri = Uri + controllerName + BuildQueryString(queryStringParams);

			var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			TOutput result;

			var response = httpWebRequest.GetResponse();
			using(var responseStream = response.GetResponseStream())
			{
				using (var stream = new StreamReader(responseStream))
				{
					var responseString = stream.ReadToEnd();
					result = JsonConvert.DeserializeObject<TOutput>(responseString);
				}
			}
			
			return result;
		}

		private static string BuildQueryString(Dictionary<string, string> queryStringParams)
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("?");

			foreach (var pair in queryStringParams)
			{
				if (string.IsNullOrEmpty(pair.Value))
				{
					continue;
				}

				if (stringBuilder.Length > 1)
				{
					stringBuilder.Append("&");
				}

				stringBuilder.Append(pair.Key).Append("=").Append(pair.Value);
			}

			return stringBuilder.ToString();
		}
	}
}
