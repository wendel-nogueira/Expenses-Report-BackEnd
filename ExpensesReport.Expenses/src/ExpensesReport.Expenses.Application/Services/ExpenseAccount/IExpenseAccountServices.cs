using ExpensesReport.Expenses.Application.InputModels.ExpenseAccountInputModel;
using ExpensesReport.Expenses.Application.ViewModels;

namespace ExpensesReport.Expenses.Application.Services.ExpenseAccount
{
    public interface IExpenseAccountServices
    {
        public Task<IEnumerable<ExpenseAccountViewModel>> GetAllExpenseAccounts();
        public Task<ExpenseAccountViewModel> GetExpenseAccountById(string id);
        public Task<ExpenseAccountViewModel> GetExpenseAccountByCode(string code);
        public Task<ExpenseAccountViewModel> AddExpenseAccount(AddExpenseAccountInputModel inputModel);
        public Task<ExpenseAccountViewModel> UpdateExpenseAccount(string id, ChangeExpenseAccountInputModel inputModel);
        public Task ActivateExpenseAccount(string id);
        public Task DeactivateExpenseAccount(string id);
    }
}
