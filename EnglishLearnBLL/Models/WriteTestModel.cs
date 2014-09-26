namespace EnglishLearnBLL.Models
{
  public class WriteTestModel
  {
    public int EnRuWordId { get; set; }
    public string Word { get; set; }
    public string Example { get; set; }
    public bool IsAnswered { get; set; }
    public string TrueAnswer { get; set; }
  }
}
