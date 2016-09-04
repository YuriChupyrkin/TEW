using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using WpfUI.Entities;

namespace WpfUI.Helpers
{
	internal class YandexTranslater
	{
		private const string ApiKey = "dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f";
		private const string Url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
		private const string TranslateLang = "en-ru";

		public static async Task<List<string>> Translate(string text)
		{
			text = HttpUtility.UrlEncode(text);

			string uri = string.Format("{0}?key={1}&lang={2}&text={3}", Url, ApiKey, TranslateLang, text);

			var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			var responseString = string.Empty;

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

			var yandexTranslateModel = JsonConvert.DeserializeObject<YandexTranslateModel>(responseString);
			var translateList = BuildTranslateList(yandexTranslateModel);

			return translateList;
		}

		private static List<string> BuildTranslateList(YandexTranslateModel model)
		{
			var list = new List<string>();

			foreach (var def in model.Def)
			{
				foreach (var tr in def.Tr)
				{
					list.Add(tr.Text);
				}
			}

			return list;
		} 
	}
}
