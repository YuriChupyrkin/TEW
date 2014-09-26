namespace WpfUI.Entities
{
  internal class TranslateWithContext
  {
    public TranslateWithContext()
    {
      Translates = new string[0];
    }

    public string[] Translates { get; set; }
    public string Context { get; set; }
  }
}
