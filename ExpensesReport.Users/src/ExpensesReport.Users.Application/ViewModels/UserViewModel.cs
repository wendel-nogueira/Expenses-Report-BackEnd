using ExpensesReport.Users.Core.Entities;

namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserViewModel(Guid id, Guid identityId, UserNameViewModel name, UserAddressViewModel address, DateTime createdAt, DateTime updatedAt)
    {
        public Guid Id { get; private set; } = id;
        public Guid IdentityId { get; private set; } = identityId;
        public UserNameViewModel Name { get; private set; } = name;
        public UserAddressViewModel Address { get; private set; } = address;
        public DateTime CreatedAt { get; private set; } = createdAt;
        public DateTime UpdatedAt { get; private set; } = updatedAt;

        public static UserViewModel FromEntity(User user)
        {
            var name = new UserNameViewModel(user.Name.FirstName, user.Name.LastName);
            var address = new UserAddressViewModel(user.Address.Address, user.Address.City, user.Address.State, user.Address.Zip, user.Address.Country);

            return new UserViewModel(user.Id, user.IdentityId, name, address, user.CreatedAt, user.UpdatedAt);
        }
    }
}
