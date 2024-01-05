using System.Data;

namespace ExpensesReport.Export.Core.Entities
{
    public class Identity
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
