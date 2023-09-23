using ExpensesReport.Users.Core.Entities;
using ExpensesReport.Users.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class AddUserInputModel
    {
        public required UserNameInputModel Name { get; set; }
        public required UserRole Role { get; set; }
        public required string Email { get; set; }
        public required UserAddressInputModel Address { get; set; }

        public User ToEntity() => new(Name.ToValueObject(), Role, Email, Address.ToValueObject());
    }
}
