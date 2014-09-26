namespace Domain.Entities
{
  public class EnRuWord : Entity<int>
  {
    public int EnglishWordId { get; set; }
    public int RussianWordId { get; set; }
    public string Context { get; set; }
    public int UserId { get; set; }
    public int WordLevel { get; set; }
    public virtual User User { get; set; }
    public virtual EnglishWord EnglishWord { get; set; }
    public virtual RussianWord RussianWord { get; set; }
  }
}
