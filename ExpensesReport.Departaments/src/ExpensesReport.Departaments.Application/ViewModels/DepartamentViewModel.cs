using ExpensesReport.Departaments.Core.Entities;

namespace ExpensesReport.Departaments.Application.ViewModels
{
    public class DepartamentViewModel(Guid id, string name, string acronym, string description, bool isDeleted, DateTime createdAt, DateTime updatedAt)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Acronym { get; set; } = acronym;
        public string Description { get; set; } = description;
        public bool IsDeleted { get; set; } = isDeleted;
        public DateTime CreatedAt { get; set; } = createdAt;
        public DateTime UpdatedAt { get; set; } = updatedAt;

        public static DepartamentViewModel FromEntity(Departament departament)
        {
            return new DepartamentViewModel(departament.Id, departament.Name, departament.Acronym, departament.Description, departament.IsDeleted, departament.CreatedAt, departament.UpdatedAt);
        }
    }
}
