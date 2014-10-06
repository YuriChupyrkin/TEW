using Newtonsoft.Json;

namespace Domain.Entities
{
  public class EnRuWord : Entity<int>
  {
    public int EnglishWordId { get; set; }
    public int RussianWordId { get; set; }
    public string Example { get; set; }
    public int UserId { get; set; }
    public int WordLevel { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }

    [JsonIgnore]
    public virtual EnglishWord EnglishWord { get; set; }

    [JsonIgnore]
    public virtual RussianWord RussianWord { get; set; }
  }
}
