using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class ChangeIdentityRoleInputModel
    {
        [Required(ErrorMessage = "Role is required!")]
        [EnumDataType(typeof(UserIdentityRole), ErrorMessage = "Role must be a valid value!")]
        public UserIdentityRole Role { get; set; }
    }
}
