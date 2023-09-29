using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email must be a valid email!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
