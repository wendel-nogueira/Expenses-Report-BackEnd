using System.Xml.Linq;

namespace ExpensesReport.Export.Core.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public Guid DepartamentId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
