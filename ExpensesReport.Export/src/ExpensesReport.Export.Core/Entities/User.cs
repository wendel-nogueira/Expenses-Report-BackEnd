using ExpensesReport.Export.Core.ValueObjects;

namespace ExpensesReport.Export.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid IdentityId { get; set; }
        public UserName? Name { get; set; }
        public UserAddress? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
