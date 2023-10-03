using ExpensesReport.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpensesReport.Users.Infrastructure.Persistence
{
    public interface IUsersDbContext
    {
        DbSet<User> Users { get; set; }
    }
}