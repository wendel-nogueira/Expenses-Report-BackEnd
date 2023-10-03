using ExpensesReport.Users.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Users.Application.InputModels
{
    public class AddUserInputModel
    {

        [Required(ErrorMessage = "Identity id is required!")]
        public Guid IdentityId { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public UserNameInputModel? Name { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public UserAddressInputModel? Address { get; set; }

        public User ToEntity() => new(IdentityId, Name!.ToValueObject(), Address!.ToValueObject());
    }
}
