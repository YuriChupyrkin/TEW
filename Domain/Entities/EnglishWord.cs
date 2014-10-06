using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Domain.Entities
{
  public class EnglishWord : Entity<int>
  {
    public string EnWord { get; set; }

    [JsonIgnore]
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}