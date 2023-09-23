using ExpensesReport.Users.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class UserAddressInputModel
    {
        [Required(ErrorMessage = "Address is required!")]
        [StringLength(100, ErrorMessage = "Address must be between 2 and 100 characters!", MinimumLength = 2)]
        public required string Address { get; set; }

        [Required(ErrorMessage = "City is required!")]
        [StringLength(50, ErrorMessage = "City must be between 2 and 50 characters!", MinimumLength = 2)]
        public required string City { get; set; }

        [Required(ErrorMessage = "State is required!")]
        [StringLength(50, ErrorMessage = "State must be between 2 and 50 characters!", MinimumLength = 2)]
        public required string State { get; set; }

        [Required(ErrorMessage = "Zip is required!")]
        [StringLength(10, ErrorMessage = "Zip must be between 2 and 10 characters!", MinimumLength = 2)]
        public required string Zip { get; set; }

        [Required(ErrorMessage = "Country is required!")]
        [StringLength(50, ErrorMessage = "Country must be between 2 and 50 characters!", MinimumLength = 2)]
        public required string Country { get; set; }

        public UserAddress ToValueObject() => new(Address, City, State, Zip, Country);
    }
}
