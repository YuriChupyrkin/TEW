using System;

namespace EnglishLearnBLL.Models
{
  public class WordJsonModel : WordViewModel
  {
    public bool IsDeleted { get; set; }
    public DateTime UpdateDate { get; set; }
    public int AnswerCount { get; set; }
    public int FailAnswerCount { get; set; }
  }
}
