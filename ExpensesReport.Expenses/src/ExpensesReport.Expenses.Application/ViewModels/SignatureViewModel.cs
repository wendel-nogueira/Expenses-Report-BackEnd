using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Application.ViewModels
{
    public class SignatureViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? Acceptance { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string? IpAddress { get; set; }
        public bool? IsDeleted { get; set; }

        public static SignatureViewModel FromEntity(Signature signature)
            => new()
            {
                Id = signature.Id,
                Name = signature.Name,
                Acceptance = signature.Acceptance,
                SignatureDate = signature.SignatureDate,
                IpAddress = signature.IpAddress,
                IsDeleted = signature.IsDeleted
            };
    }
}
