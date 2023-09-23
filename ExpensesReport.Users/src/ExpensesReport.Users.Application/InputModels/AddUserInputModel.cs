using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class AddUserInputModel
    {
        public required UserNameInputModel Name { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be a valid value!")]
        public required UserRole Role { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email must be a valid email!")]
        public required string Email { get; set; }
        public required UserAddressInputModel Address { get; set; }

        public User ToEntity() => new(Name.ToValueObject(), Role, Email, Address.ToValueObject());
    }
}
