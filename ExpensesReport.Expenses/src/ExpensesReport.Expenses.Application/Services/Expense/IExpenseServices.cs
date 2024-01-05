using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Application.ViewModels;

namespace ExpensesReport.Expenses.Application.Services.Expense
{
    public interface IExpenseServices
    {
        public Task<IEnumerable<ExpenseViewModel>> GetAllExpenses();
        public Task<ExpenseViewModel> GetExpenseById(string id);
        public Task<IEnumerable<ExpenseViewModel>> GetExpensesByExpenseReportId(string expenseReportId);
        public Task<ExpenseViewModel> AddExpense(string expenseReportId, AddExpenseInputModel inputModel);
        public Task<ExpenseViewModel> UpdateExpense(string id, ChangeExpenseInputModel inputModel);
        public Task DeleteExpense(string id);
        public Task EvaluateExpense(string id, EvaluateInputModel inputModel);
    }
}