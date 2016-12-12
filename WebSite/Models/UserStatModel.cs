using System;

namespace TewCloud.Models
{
  public class UserStatModel
  {
    public DateTime LastActivityDate { get; set; }

    public long ActivityLevel { get; set; }

    public long WordsLevel { get; set; }

    public string Email { get; set; }

    public string NickName { get; set; }
  }
}