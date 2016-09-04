namespace WpfUI.Entities
{
	internal class YandexTranslateModel
	{
		public Def[] Def { get; set; }
	}

	internal class Def
	{
		public string Text { get; set; }

		public Tr[] Tr { get; set; }
	}

	internal class Tr
	{
		public string Text { get; set; }

		public Syn[] Syn { get; set; }
	}

	internal class Syn
	{
		public string Text { get; set; }
	}
}
