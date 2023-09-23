using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;

namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(Guid id, UserNameViewModel name, UserRole role, string email, UserAddressViewModel address, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Role = role;
            Email = email;
            Address = address;
            CreatedAt = createdAt;
        }

        public Guid Id { get; private set; }
        public UserNameViewModel Name { get; private set; }
        public UserRole Role { get; private set; }
        public string Email { get; private set; }
        public UserAddressViewModel Address { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static UserViewModel FromEntity(User user)
        {
            var name = new UserNameViewModel(user.Name.FirstName, user.Name.LastName);
            var address = new UserAddressViewModel(user.Address.Address, user.Address.City, user.Address.State, user.Address.Zip, user.Address.Country);

            return new UserViewModel(user.Id, name, user.Role, user.Email, address, user.CreatedAt);
        }
    }
}
