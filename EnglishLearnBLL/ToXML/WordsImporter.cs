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
      string fileName = "Test";
      XDocument doc = XDocument.Load(fileName);

      XElement userEl = doc.Root.Elements().First();
      var userName = userEl.Value;

      var user = _repositoryFactory.UserRepository.Find(r => r.Email == userName);

      if (user == null || user.Email != forUserName)
      {
        return;
      }

      IEnumerable<XElement> words = doc.Root.Elements().Skip(1).Take(1);

      foreach (XElement word in words)
      {
        foreach (XElement wordElemets in word.Elements())
        {
          var enWord = wordElemets.Element("engWord").Value;
          var ruWord = wordElemets.Element("rusWord").Value;
          var level = int.Parse(wordElemets.Element("level").Value);
          var example = wordElemets.Element("example").Value;

          _repositoryFactory.EnRuWordsRepository
            .AddTranslate(enWord, ruWord, example, user.Id, level);
        }
      }
    }

  }
}
