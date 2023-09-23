using ExpensesReport.Users.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class UserNameInputModel
    {
        [Required(ErrorMessage = "First name is required!")]
        [StringLength(50, ErrorMessage = "First name must be between 2 and 50 characters!", MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(50, ErrorMessage = "Last name must be between 2 and 50 characters!", MinimumLength = 2)]
        public required string LastName { get; set; }

        public UserName ToValueObject() => new(FirstName, LastName);
    }
}
