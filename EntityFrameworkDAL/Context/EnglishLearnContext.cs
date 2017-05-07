using System.Data.Entity;
using Common;
using Domain.Entities;

namespace EntityFrameworkDAL.Context
{
  public class EnglishLearnContext : DbContext
  {
    internal EnglishLearnContext()
			: base(GlobalConfiguration.IsDevelopmentEnvironment ? "EnglishLearnContext_Dev" : "EnglishLearnContext")
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

  }
}