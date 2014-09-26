using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Xml.Linq;

namespace WpfUI.Helpers
{
  internal class BingTranslater
  {
    //private const string FromLang = "en";
    //private const string ToLang = "ru";
    //private const string Url = "http://api.microsofttranslator.com/v2/Http.svc/Translate";

    //public async Task<string> GetTranslate(string word)
    //{
    //  var auth = ApplicationContext.AdminAuthentication;

    //  string uri = Url + "?text=" + HttpUtility.UrlEncode(word)
    //    + "&from=" + FromLang + "&to=" + ToLang;

    //  string authToken = "Bearer" + " " + auth.GetAccessToken().access_token;

    //  var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
    //  httpWebRequest.Headers.Add("Authorization", authToken);

    //  try
    //  {
    //    var response = await httpWebRequest.GetResponseAsync();
    //    using (Stream stream = response.GetResponseStream())
    //    {
    //      var dcs = new DataContractSerializer(Type.GetType("System.String"));
    //      var translation = (string)dcs.ReadObject(stream);
    //      return translation;
    //    }
    //  }
    //  catch
    //  {
    //    //swallow exceptions
    //    MainWindow.SetAdminAuthentication();
    //  }
    //  return string.Empty;
    //}
  }
}
