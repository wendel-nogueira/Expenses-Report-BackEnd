using ExpensesReport.Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Identity.Infrastructure.Persistence.Context
{
    public interface IUserIdentityDbContext
    {
        public DbSet<UserIdentity> Identities { get; set; }
    }
}
