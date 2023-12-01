using ExpensesReport.Expenses.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.SignatureInputModel
{
    public class AddSignatureInputModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Acceptance is required!")]
        public bool? Acceptance { get; set; }

        [Required(ErrorMessage = "SignatureDate is required!")]
        [DataType(DataType.Date)]
        public DateTime? SignatureDate { get; set; }

        [Required(ErrorMessage = "IpAddress is required!")]
        [StringLength(50, ErrorMessage = "IpAddress must be less than 50 characters!")]
        public string? IpAddress { get; set; }

        public Signature ToEntity() => new(Name!, Convert.ToBoolean(Acceptance), SignatureDate!.Value, IpAddress!);
    }
}
