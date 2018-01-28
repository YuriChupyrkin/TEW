namespace EnglishLearnBLL.Models
{
  public class WordUpdateModel
  {
    public int WordId { get; set; }

    public bool IsTrueAnswer { get; set; }

    public WordLevelManager.WordLevelManager.TestType TestType { get; set; }
  }
}
