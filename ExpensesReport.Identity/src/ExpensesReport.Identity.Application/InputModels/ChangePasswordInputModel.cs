using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class ChangePasswordInputModel
    {
        [Required(ErrorMessage = "New password is required!")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Repeat new password is required!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords must match!")]
        public string? RepeatNewPassword { get; set; }
    }
}
