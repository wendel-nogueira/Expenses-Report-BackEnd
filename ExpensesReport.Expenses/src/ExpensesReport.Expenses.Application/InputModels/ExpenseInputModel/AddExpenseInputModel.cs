using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel
{
    public class AddExpenseInputModel
    {
        [Required(ErrorMessage = "ExpenseAccount is required!")]
        public string? ExpenseAccount { get; set; }

        [Required(ErrorMessage = "Amount is required!")]
        [Range(0, 99999.99, ErrorMessage = "Amount must be a valid value!")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "DateIncurred is required!")]
        [DataType(DataType.DateTime)]
        public DateTime? DateIncurred { get; set; }

        [Required(ErrorMessage = "DateIncurredTimeZone is required!")]
        public string? DateIncurredTimeZone { get; set; }

        [Required(ErrorMessage = "Explanation is required!")]
        [StringLength(100, ErrorMessage = "Explanation must be less than 100 characters!")]
        public string? Explanation { get; set; }

        [Required(ErrorMessage = "Receipt is required!")]
        [StringLength(100, ErrorMessage = "Receipt must be less than 100 characters!")]
        public string? Receipt { get; set; }

        public Expense ToEntity() => new(ExpenseAccount!, Amount!.Value, DateIncurred!.Value, Explanation!, null, null, null, "", Receipt!, DateIncurredTimeZone, null);
    }
}
