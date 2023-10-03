namespace ExpensesReport.Users.Core.ValueObjects
{
    public record UserAddress
    {
        public UserAddress(string address, string city, string state, string zip, string country)
        {
            Address = address;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
        }

        public string Address { get; init; }

        public string City { get; init; }

        public string State { get; init; }

        public string Zip { get; init; }

        public string Country { get; init; }

        public override string ToString()
        {
            return $"{Address}, {City}, {State}, {Zip}, {Country}";
        }
    }
}
