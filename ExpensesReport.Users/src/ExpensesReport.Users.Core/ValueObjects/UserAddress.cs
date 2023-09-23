using System.ComponentModel.DataAnnotations;

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

            Validate();
        }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, ErrorMessage = "Address must be between 2 and 100 characters", MinimumLength = 2)]
        public string Address { get; init; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City must be between 2 and 50 characters", MinimumLength = 2)]
        public string City { get; init; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State must be between 2 and 50 characters", MinimumLength = 2)]
        public string State { get; init; }

        [Required(ErrorMessage = "Zip is required")]
        [StringLength(10, ErrorMessage = "Zip must be between 2 and 10 characters", MinimumLength = 2)]
        public string Zip { get; init; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country must be between 2 and 50 characters", MinimumLength = 2)]
        public string Country { get; init; }

        public override string ToString()
        {
            return $"{Address}, {City}, {State}, {Zip}, {Country}";
        }

        public void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, context, results, true))
                throw new Exception(string.Join(" | ", results.Select(x => x.ErrorMessage)));
        }
    }
}
