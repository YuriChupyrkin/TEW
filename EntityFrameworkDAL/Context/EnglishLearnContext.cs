using System.Configuration;
using System.Data.Entity;
using Domain.Entities;

namespace EntityFrameworkDAL.Context
{
  public class EnglishLearnContext : DbContext
  {
    internal EnglishLearnContext() : base(DataBaseName)
    {
      Database.SetInitializer(new EnglishLearnContextInitializer());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<EnglishWord> EnglishWords { get; set; }
    public DbSet<RussianWord> RussianWords { get; set; }
    public DbSet<EnRuWord> EnRuWords { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Configurations.Add(new EnRuWordsMapper());
    }
    
    private static string DataBaseName
    {
      get {
        var dataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
        return dataBaseName;
      }
    }
  }
}