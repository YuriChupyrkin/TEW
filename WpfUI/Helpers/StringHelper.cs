namespace WpfUI.Helpers
{
  internal static class StringHelper
  {
    public static string AsteriskReplace(this string sentence, string partOfWord)
    {
      var startIndex = sentence.IndexOf(partOfWord, 
        System.StringComparison.OrdinalIgnoreCase);

      if (startIndex == -1)
      {
        return sentence;
      }

      var asterisks = "";
      for (var i = 0; i < partOfWord.Length; i++)
      {
        asterisks += "*";
      }

      sentence = sentence.Replace(partOfWord, asterisks);
      return sentence;
    }
  }
}
