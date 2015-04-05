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

		public void Import(string forUserName, string fileName)
		{
			XDocument doc = XDocument.Load(fileName);

			var user = _repositoryFactory.UserRepository.Find(r => r.Email == forUserName);

			try
			{

				IEnumerable<XElement> words = doc.Root.Elements().Take(1);

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
