namespace ExpensesReport.Users.Application.ViewModels
{
    public class UserAddressViewModel
    {
        public UserAddressViewModel(string address, string city, string state, string zip, string country)
        {
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
        }

        public string Address { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }
        public string Country { get; private set; }
    }
}
