namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class UserViewModel(Guid id, Guid identityId, UserNameViewModel name, UserAddressViewModel address, DateTime createdAt, DateTime updatedAt)
    {
        public Guid Id { get; private set; } = id;
        public Guid IdentityId { get; private set; } = identityId;
        public UserNameViewModel Name { get; private set; } = name;
        public UserAddressViewModel Address { get; private set; } = address;
        public DateTime CreatedAt { get; private set; } = createdAt;
        public DateTime UpdatedAt { get; private set; } = updatedAt;
    }
}