using System.Collections.ObjectModel;

namespace Domain.Entities
{
  public class RussianWord : Entity<int>
  {
    public string RuWord { get; set; }
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}