using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel
{
    public class AddExpenseInputModel
    {
        [Required(ErrorMessage = "Amount is required!")]
        [Range(0, 99999.99, ErrorMessage = "Amount must be a valid value!")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "DateIncurred is required!")]
        [DataType(DataType.DateTime)]
        public DateTime? DateIncurred { get; set; }

        [Required(ErrorMessage = "Explanation is required!")]
        [StringLength(100, ErrorMessage = "Explanation must be less than 100 characters!")]
        public string? Explanation { get; set; }

        [Required(ErrorMessage = "Status is required!")]
        [EnumDataType(typeof(ExpenseStatus), ErrorMessage = "Status must be a valid value!")]
        public ExpenseStatus? Status { get; set; }

        [Required(ErrorMessage = "ActionById is required!")]
        public Guid? ActionById { get; set; }

        [Required(ErrorMessage = "ActionDate is required!")]
        [DataType(DataType.DateTime)]
        public DateTime? ActionDate { get; set; }

        [Required(ErrorMessage = "AccountingNotes is required!")]
        public string? AccountingNotes { get; set; }

        [Required(ErrorMessage = "Receipt is required!")]
        [StringLength(100, ErrorMessage = "Receipt must be less than 100 characters!")]
        public string? Receipt { get; set; }

        public Expense ToEntity() => new(Amount!.Value, DateIncurred!.Value, Explanation!, Status!.Value, ActionById!.Value, ActionDate!.Value, AccountingNotes!, Receipt!);
    }
}
