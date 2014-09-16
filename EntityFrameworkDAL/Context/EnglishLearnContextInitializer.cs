using System.Collections.Generic;
using System.Data.Entity;
using Domain.Entities;

namespace EntityFrameworkDAL.Context
{
  internal class EnglishLearnContextInitializer
    : DropCreateDatabaseIfModelChanges<EnglishLearnContext>

  {
    protected override void Seed(EnglishLearnContext context)
    {
      var roles = new List<Role>
      {
        new Role {RoleName = "user"},
        new Role {RoleName = "admin"},
        new Role {RoleName = "baned"}
      };

      roles.ForEach(r => context.Roles.Add(r));

      var user = new User
      {
        Email = "admin@admin.admin",
        Password = "admin",
        RoleId = 1
      };

      context.Users.Add(user);

      context.SaveChanges();
    }
  }
}