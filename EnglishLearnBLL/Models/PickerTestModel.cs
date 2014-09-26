using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
  public class PickerTestModel
  {
    public PickerTestModel()
    {
      Answers = new List<string>();
    }
    public int WordId { get; set; }
    public string Word { get; set; }
    public string Example { get; set; }
    public IList<string> Answers { get; set; }
    public int AnswerId { get; set; }
  }
}
