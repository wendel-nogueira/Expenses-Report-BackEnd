using System.Xml.Linq;

namespace ExpensesReport.Export.Core.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Acronym { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
