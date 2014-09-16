using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace EntityFrameworkDAL.Context
{
  internal class EnRuWordsMapper : EntityTypeConfiguration<EnRuWord>
  {
    internal EnRuWordsMapper()
    {
      HasKey(x => x.Id);
      Property(x => x.Id)
        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

      HasRequired(x => x.User).WithMany(x => x.EnRuWords).WillCascadeOnDelete();
      HasRequired(x => x.EnglishWord).WithMany(x => x.EnRuWords).WillCascadeOnDelete();
      HasRequired(x => x.RussianWord).WithMany(x => x.EnRuWords).WillCascadeOnDelete();
    }
  }
}