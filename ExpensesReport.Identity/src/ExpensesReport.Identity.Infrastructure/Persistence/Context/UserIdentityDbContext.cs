using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Identity.Infrastructure.Persistence.Context
{
    public class UserIdentityDbContext : IdentityDbContext, IUserIdentityDbContext
    {
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options) { }

        public DbSet<UserIdentity> Identities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var roleEnums = UserIdentityRoleExtensions.GetValues();

            foreach (var role in roleEnums)
            {
                builder.Entity<IdentityRole>().HasData(new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = role.ToString(),
                    NormalizedName = role.ToString().ToUpper(),
                });
            }

            base.OnModelCreating(builder);
        }
    }
}
