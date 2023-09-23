using ExpensesReport.Users.Core.ValueObjects;

namespace ExpensesReport.Users.Application.InputModels
{
    public class UserAddressInputModel
    {
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Zip { get; set; }
        public required string Country { get; set; }

        public UserAddress ToValueObject() => new(Address, City, State, Zip, Country);
    }
}
