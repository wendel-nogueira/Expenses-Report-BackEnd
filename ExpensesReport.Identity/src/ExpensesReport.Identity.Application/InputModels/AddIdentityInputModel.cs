using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Xml.Linq;

namespace ExpensesReport.Identity.Application.InputModels
{
    public class AddIdentityInputModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email must be a valid email!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        [EnumDataType(typeof(UserIdentityRole), ErrorMessage = "Role must be a valid value!")]
        public UserIdentityRole? Role { get; set; }

        public (UserIdentity, IdentityRole) ToEntity() => (new UserIdentity(), new IdentityRole(Role!.ToString()!));
    }
}
