using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Enums;
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

            var expense = inputModel.ToEntity();
            var expenseReport = await expenseReportRepository.GetByIdAsync(expenseReportId) ?? throw new NotFoundException("Expense report not found!");
            expenseReport.TotalAmount += expense.Amount;

            expenseReport.Expenses.Add(expense);

            await expenseReportRepository.UpdateAsync(expenseReport);

            return ExpenseViewModel.FromEntity(expense);
        }

        public async Task<ExpenseViewModel> UpdateExpense(string id, ChangeExpenseInputModel inputModel)
        {
            var expenseReport = await expenseRepository.GetExpenseReportByExpenseIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            _ = await expenseAccountRepository.GetByIdAsync(inputModel.ExpenseAccount!) ?? throw new NotFoundException("Expense account not found!");

            var expenseToUpdate = await expenseRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense not found!");
            expenseReport.Expenses.Remove(expenseToUpdate);

            expenseToUpdate.Update(inputModel.ExpenseAccount!, inputModel.Amount!.Value, inputModel.DateIncurred!.Value, inputModel.Explanation!, inputModel.Status!.Value, inputModel.AccountingNotes!, inputModel.Receipt!, inputModel.DateIncurredTimeZone);

            if (expenseToUpdate.Amount != inputModel.Amount)
            {
                expenseReport.TotalAmount -= expenseToUpdate.Amount;
                expenseReport.TotalAmount += inputModel.Amount!.Value;
            }

            if (expenseToUpdate.Status != inputModel.Status)
            {
                if (expenseToUpdate.Status == ExpenseStatus.Approved)
                {
                    expenseReport.AmountApproved -= expenseToUpdate.Amount;
                }
                else if (expenseToUpdate.Status == ExpenseStatus.Rejected)
                {
                    expenseReport.AmountRejected -= expenseToUpdate.Amount;
                }

                if (inputModel.Status == ExpenseStatus.Approved)
                {
                    expenseReport.AmountApproved += inputModel.Amount!.Value;
                }
                else if (inputModel.Status == ExpenseStatus.Rejected)
                {
                    expenseReport.AmountRejected += inputModel.Amount!.Value;
                }
            }

            expenseReport.Expenses.Add(expenseToUpdate);

            return ExpenseViewModel.FromEntity(expenseToUpdate);
        }

        public async Task DeleteExpense(string id)
        {
            var expenseReport = await expenseRepository.GetExpenseReportByExpenseIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            var expense = expenseReport.Expenses.FirstOrDefault(e => e.Id == id) ?? throw new NotFoundException("Expense not found!");

            expenseReport.TotalAmount -= expense.Amount;

            if (expense.Status == ExpenseStatus.Approved)
            {
                expenseReport.AmountApproved -= expense.Amount;
            }
            else if (expense.Status == ExpenseStatus.Rejected)
            {
                expenseReport.AmountRejected -= expense.Amount;
            }

            expenseReport.Expenses.Remove(expense);

            await expenseReportRepository.UpdateAsync(expenseReport);
        }

        public async Task EvaluateExpense(string id, EvaluateInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on evaluate expense!", errorsInput);
            }

            var expenseReport = await expenseRepository.GetExpenseReportByExpenseIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            var expenseToEvaluate = expenseReport.Expenses.FirstOrDefault(e => e.Id == id) ?? throw new NotFoundException("Expense not found!");

            if (expenseToEvaluate.Status == ExpenseStatus.Approved || expenseToEvaluate.Status == ExpenseStatus.Rejected)
            {
                throw new BadRequestException("Expense already evaluated!", []);
            }

            expenseReport.Expenses.Remove(expenseToEvaluate);
            expenseToEvaluate.Evaluate(inputModel.Status!.Value, new Guid(inputModel.ActionBy!), inputModel.ActionDate!.Value, inputModel.AccountingNotes!, inputModel.ActionDateTimeZone);

            if (expenseToEvaluate.Status == ExpenseStatus.Approved)
            {
                expenseReport.AmountApproved += expenseToEvaluate.Amount;
            }
            else if (expenseToEvaluate.Status == ExpenseStatus.Rejected)
            {
                expenseReport.AmountRejected += expenseToEvaluate.Amount;
            }

            expenseReport.Expenses.Add(expenseToEvaluate);

            await expenseReportRepository.UpdateAsync(expenseReport);
        }
    }
}
