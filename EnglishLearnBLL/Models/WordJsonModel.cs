using System;

namespace EnglishLearnBLL.Models
{
  public class WordJsonModel : WordViewModel
  {
    public bool IsDeleted { get; set; }
    public DateTime UpdateDate { get; set; }
  }
}
