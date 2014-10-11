using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Domain.RepositoryFactories;

namespace EnglishLearnBLL.ToXML
{
  public class WordsImporter
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public WordsImporter(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public void Import(string forUserName)
    {
      string fileName = XmlNameHelper.XmlFileName;
      XDocument doc = XDocument.Load(fileName);

      XElement userEl = doc.Root.Elements().First();
      var userName = userEl.Value;

      var user = _repositoryFactory.UserRepository.Find(r => r.Email == userName);

      if (user == null || user.Email != forUserName)
      {
        throw new Exception("Words import error! Incorrect user info");
      }

      try
      {

        IEnumerable<XElement> words = doc.Root.Elements().Skip(1).Take(1);

        foreach (XElement word in words)
        {
          foreach (XElement wordElemets in word.Elements())
          {
            var enWord = wordElemets.Element(XmlNameHelper.EngWord).Value;
            var ruWord = wordElemets.Element(XmlNameHelper.RusWord).Value;
            var level = int.Parse(wordElemets.Element(XmlNameHelper.Level).Value);
            var example = wordElemets.Element(XmlNameHelper.Example).Value;

            _repositoryFactory.EnRuWordsRepository
              .AddTranslate(enWord, ruWord, example, user.Id, new DateTime(1990, 5, 5), level);
          }
        }
      }
      catch
      {
        throw new Exception("Words import error! file error");
      }
    }
  }
}
