namespace ExpensesReport.Projects.Core.Entities
{
    public class Project : EntityBase
    {
        [Obsolete("Parameterless constructor should not be used directly.")]

        public Project() { }

        public Project(string name, string code, string description, Guid departmentId)
        {
            Name = name;
            Code = code;
            Description = description;
            DepartamentId = departmentId;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid DepartamentId { get; set; }

        public void Update(string name, string code, string description, Guid departmentId)
        {
            Name = name;
            Code = code;
            Description = description;
            DepartamentId = departmentId;

            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.Now;
        }
    }
}
