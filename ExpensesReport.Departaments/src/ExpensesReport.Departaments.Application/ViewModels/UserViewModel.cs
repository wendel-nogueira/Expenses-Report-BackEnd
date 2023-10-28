using ExpensesReport.Departaments.Core.Entities;

namespace ExpensesReport.Departaments.Application.ViewModels
{
    public class UserViewModel(IEnumerable<Guid> usersId)
    {
        public IEnumerable<Guid> UsersId { get; set; } = usersId;

        public static UserViewModel FromEntity(IEnumerable<User> users)
        {
            return new UserViewModel(users.Select(user => user.UserId));
        }
    }
}
