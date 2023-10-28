using ExpensesReport.Projects.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Projects.Application.InputModels
{
    public class AddProjectInputModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(50, ErrorMessage = "Name must have a maximum of 50 characters!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Code is required!")]
        [MaxLength(10, ErrorMessage = "Code must have a maximum of 10 characters!")]
        public string? Code { get; set; }

        [MaxLength(200, ErrorMessage = "Description must have a maximum of 200 characters!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "DepartmentId is required!")]
        public Guid DepartamentId { get; set; }

        public Project ToEntity() => new(Name!, Code!, Description!, DepartamentId!);
    }
}
