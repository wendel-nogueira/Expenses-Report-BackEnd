using ExpensesReport.Departaments.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Departaments.Application.InputModels
{
    public class AddDepartamentInputModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(50, ErrorMessage = "Name must have a maximum of 50 characters!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Acronym is required!")]
        [MaxLength(10, ErrorMessage = "Acronym must have a maximum of 10 characters!")]
        public string? Acronym { get; set; }

        [MaxLength(200, ErrorMessage = "Description must have a maximum of 200 characters!")]
        public string? Description { get; set; }

        public Departament ToEntity() => new(Name!, Acronym!, Description!);
    }
}
