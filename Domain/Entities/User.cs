using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System;

namespace Domain.Entities
{
  public class User : Entity<int>
  {
    public string Email { get; set; }

    public string Password { get; set; }

    public virtual Role Role { get; set; }

    public int RoleId { get; set; }

    public DateTime LastActivity { get; set; }

    public long ActivityLevel { get; set; }

    public string NickName { get; set; }

    [JsonIgnore]
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}