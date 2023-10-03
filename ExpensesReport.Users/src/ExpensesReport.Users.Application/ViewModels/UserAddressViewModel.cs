namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserAddressViewModel(string address, string city, string state, string zip, string country)
    {
        public string Address { get; private set; } = address;
        public string City { get; private set; } = city;
        public string State { get; private set; } = state;
        public string Zip { get; private set; } = zip;
        public string Country { get; private set; } = country;
    }
}
