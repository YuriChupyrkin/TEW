using System.Collections.ObjectModel;

namespace Domain.Entities
{
  public class User : Entity<int>
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public virtual Role Role { get; set; }
    public int RoleId { get; set; }
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}