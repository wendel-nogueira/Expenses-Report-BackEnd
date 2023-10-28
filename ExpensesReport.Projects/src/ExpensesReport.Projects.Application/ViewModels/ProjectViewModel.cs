using ExpensesReport.Projects.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpensesReport.Projects.Application.ViewModels
{
    public class ProjectViewModel(Guid id, string name, string code, string description, Guid departamentId, bool isDeleted, DateTime createdAt, DateTime updatedAt)
    {
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Code { get; set; } = code;
        public string Description { get; set; } = description;
        public Guid DepartamentId { get; set; } = departamentId;
        public bool IsDeleted { get; set; } = isDeleted;
        public DateTime CreatedAt { get; set; } = createdAt;
        public DateTime UpdatedAt { get; set; } = updatedAt;

        public static ProjectViewModel FromEntity(Project project)
        {
            return new ProjectViewModel(project.Id, project.Name, project.Code, project.Description, project.DepartamentId, project.IsDeleted, project.CreatedAt, project.UpdatedAt);
        }
    }
}
