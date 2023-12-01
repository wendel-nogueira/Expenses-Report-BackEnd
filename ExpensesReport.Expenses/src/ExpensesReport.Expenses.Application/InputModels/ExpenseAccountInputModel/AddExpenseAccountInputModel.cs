using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Expenses.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ExpensesReport.Expenses.Application.InputModels.ExpenseAccountInputModel
{
    public class AddExpenseAccountInputModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Code is required!")]
        [StringLength(10, ErrorMessage = "Code must be less than 10 characters!")]
        public string? Code { get; set; }

        [StringLength(100, ErrorMessage = "Description must be less than 100 characters!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Type is required!")]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Type must be a valid value!")]
        public AccountType? Type { get; set; }

        public ExpenseAccount ToEntity() => new(Name!, Code!, Description!, Type!.Value);
    }
}
