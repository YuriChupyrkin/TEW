using System.Collections.Generic;

namespace EnglishLearnBLL.Models
{
  public sealed class ResponseModel
  {
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }
    public WordsCloudModel WordsCloudModel { get; set; } 
  }
}
