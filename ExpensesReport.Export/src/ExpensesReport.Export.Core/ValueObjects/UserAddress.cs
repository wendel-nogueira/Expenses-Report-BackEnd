namespace ExpensesReport.Export.Core.ValueObjects
{
    public class UserAddress
    {
        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zip { get; set; }

        public string? Country { get; set; }

        public override string ToString()
        {
            return $"{Address}, {City}, {State}, {Country}, {Zip}";
        }
    }
}
