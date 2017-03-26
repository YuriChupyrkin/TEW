namespace WebSite.Models
{
  public class GetUserWordsModel
  {
    public int UserId { get; set; }

    public int CurrentWordsCount { get; set; }

    public int WordsPerPage { get; set; }

    public string SortKey { get; set; }

    public bool SortAsc { get; set; }
  }
}