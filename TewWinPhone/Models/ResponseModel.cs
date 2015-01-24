using TewWinPhone.Models;

namespace TewWinPhone.Models
{
  public sealed class ResponseModel
  {
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }
    public SyncWordsModel WordsCloudModel { get; set; } 
  }
}
