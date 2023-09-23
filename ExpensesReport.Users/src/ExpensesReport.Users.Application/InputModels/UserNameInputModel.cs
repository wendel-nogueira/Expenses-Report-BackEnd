using ExpensesReport.Users.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class UserNameInputModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public UserName ToValueObject() => new(FirstName, LastName);
    }
}
