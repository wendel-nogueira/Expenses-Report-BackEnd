using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Repositories;

namespace ExpensesReport.Expenses.Application.Services.Expense
{
    public class ExpenseServices(IExpenseRepository expenseRepository, IExpenseReportRepository expenseReportRepository, IExpenseAccountRepository expenseAccountRepository) : IExpenseServices
    {
        public async Task<IEnumerable<ExpenseViewModel>> GetAllExpenses()
        {
            var expenses = await expenseRepository.GetAllAsync();
            return expenses.Select(ExpenseViewModel.FromEntity);
        }

        public async Task<ExpenseViewModel> GetExpenseById(string id)
        {
            var expense = await expenseRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense not found!");
            return ExpenseViewModel.FromEntity(expense);
        }

        public async Task<IEnumerable<ExpenseViewModel>> GetExpensesByExpenseReportId(string expenseReportId)
        {
            var expenses = await expenseRepository.GetAllInExpenseReportAsync(expenseReportId);

            return expenses.Select(ExpenseViewModel.FromEntity);
        }

        public async Task<ExpenseViewModel> AddExpense(string expenseReportId, AddExpenseInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create expense!", errorsInput);
            }

            _ = await expenseAccountRepository.GetByIdAsync(inputModel.ExpenseAccount!) ?? throw new NotFoundException("Expense account not found!");
            var expenseReport = await expenseReportRepository.GetByIdAsync(expenseReportId) ?? throw new NotFoundException("Expense report not found!");
            var expense = inputModel.ToEntity();

            await expenseRepository.AddAsync(expenseReport.Id, expense);

            return ExpenseViewModel.FromEntity(expense);
        }

        public async Task<ExpenseViewModel> UpdateExpense(string id, ChangeExpenseInputModel inputModel)
        {
            var expenseReport = await expenseRepository.GetExpenseReportByExpenseIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            var expense = await expenseRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense not found!");
            _ = await expenseAccountRepository.GetByIdAsync(inputModel.ExpenseAccount!) ?? throw new NotFoundException("Expense account not found!");

            expense.Update(inputModel.ExpenseAccount!, inputModel.Amount!.Value, inputModel.DateIncurred!.Value, inputModel.Explanation!, inputModel.Status!.Value, inputModel.AccountingNotes!, inputModel.Receipt!, inputModel.DateIncurredTimeZone);

            await expenseRepository.UpdateAsync(expenseReport.Id, expense);

            return ExpenseViewModel.FromEntity(expense);
        }

        public async Task DeleteExpense(string id)
        {
            var expenseReport = await expenseRepository.GetExpenseReportByExpenseIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            var expense = await expenseRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense not found!");

            await expenseRepository.DeleteAsync(expenseReport.Id, expense);
        }
    }
}
