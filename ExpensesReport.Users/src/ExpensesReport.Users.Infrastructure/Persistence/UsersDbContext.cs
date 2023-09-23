using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Users.Infrastructure.Persistence
{
    public class UsersDbContext
    {
        public List<User> Users { get; set; }

        public UsersDbContext()
        {
            Users = new List<User>();
        }
    }
}
