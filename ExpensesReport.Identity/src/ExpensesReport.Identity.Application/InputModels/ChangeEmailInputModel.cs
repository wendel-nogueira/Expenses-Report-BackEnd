using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class ChangeEmailInputModel
    {
        [Required(ErrorMessage = "New email is required!")]
        [EmailAddress(ErrorMessage = "New email must be a valid email!")]
        public string? NewEmail { get; set; }
    }
}
