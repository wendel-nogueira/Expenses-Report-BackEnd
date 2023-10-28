using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class ResetPasswordInputModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email must be a valid email!")]
        public string? Email { get; set; }
    }
}
