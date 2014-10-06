using System.Linq;
using System.Xml.Linq;
using Domain.RepositoryFactories;

namespace EnglishLearnBLL.ToXML
{
  public class WordsExporter
  {
    private readonly IRepositoryFactory _repositoryFactory;

    public WordsExporter(IRepositoryFactory repositoryFactory)
    {
      _repositoryFactory = repositoryFactory;
    }

    public void Export(int userId)
    {
      var doc = new XDocument();
      var userWordsEl = new XElement("userWords");
      doc.Add(userWordsEl);

      var wordsFromDb = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
        .Where(r => r.UserId == userId);

      if (wordsFromDb.Any() == false)
      {
        return;
      }

      var userEl = new XElement("user");
      userEl.Value = wordsFromDb.First().User.Email;
      doc.Root.Add(userEl);

      var wordsEl = new XElement("words");
      doc.Root.Add(wordsEl);

      foreach (var enRuWord in wordsFromDb)
      {
        var wordEl = new XElement("word");

        var engWord = new XElement("engWord");
        engWord.Value = enRuWord.EnglishWord.EnWord;
        wordEl.Add(engWord);

        var rusWord = new XElement("rusWord");
        rusWord.Value = enRuWord.RussianWord.RuWord;
        wordEl.Add(rusWord);

        var level = new XElement("level");
        level.Value = enRuWord.WordLevel.ToString();
        wordEl.Add(level);

        var example = new XElement("example");
        example.Value = enRuWord.Example;
        wordEl.Add(example);

        wordsEl.Add(wordEl);
      }

      doc.Save("Test");
    }
  }
}
