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

		public void Export(int userId, string fileName)
		{
			var doc = new XDocument();
			var userWordsEl = new XElement(XmlNameHelper.UserWords);
			doc.Add(userWordsEl);

			var wordsFromDb = _repositoryFactory.EnRuWordsRepository.AllEnRuWords()
			  .Where(r => r.UserId == userId);

			if (wordsFromDb.Any() == false)
			{
				return;
			}

			//var userEl = new XElement(XmlNameHelper.User);
			//userEl.Value = wordsFromDb.First().User.Email;
			//doc.Root.Add(userEl);

			var wordsEl = new XElement(XmlNameHelper.Words);
			doc.Root.Add(wordsEl);

			foreach (var enRuWord in wordsFromDb)
			{
				var wordEl = new XElement(XmlNameHelper.Word);

				var engWord = new XElement(XmlNameHelper.EngWord);
				engWord.Value = enRuWord.EnglishWord.EnWord;
				wordEl.Add(engWord);

				var rusWord = new XElement(XmlNameHelper.RusWord);
				rusWord.Value = enRuWord.RussianWord.RuWord;
				wordEl.Add(rusWord);

				var level = new XElement(XmlNameHelper.Level);
				level.Value = "0"; //enRuWord.WordLevel.ToString();
				wordEl.Add(level);

				var example = new XElement(XmlNameHelper.Example);
				example.Value = enRuWord.Example;
				wordEl.Add(example);

				wordsEl.Add(wordEl);
			}

			doc.Save(fileName);
		}
	}
}
