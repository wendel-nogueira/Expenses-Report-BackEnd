namespace ExpensesReport.Export.Core.Entities
{
    public class Signature
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? Acceptance { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string? IpAddress { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
