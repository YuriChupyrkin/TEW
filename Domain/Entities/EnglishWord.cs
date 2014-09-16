using System.Collections.ObjectModel;

namespace Domain.Entities
{
  public class EnglishWord : Entity<int>
  {
    public string EnWord { get; set; }
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}