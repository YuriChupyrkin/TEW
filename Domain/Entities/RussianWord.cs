using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace Domain.Entities
{
  public class RussianWord : Entity<int>
  {
    public string RuWord { get; set; }

    [JsonIgnore]
    public virtual Collection<EnRuWord> EnRuWords { get; set; }
  }
}