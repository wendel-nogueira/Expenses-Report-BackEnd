using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseAccountInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Expenses.Core.Repositories;

namespace ExpensesReport.Expenses.Application.Services.ExpenseAccount
{
    public class ExpenseAccountServices(IExpenseAccountRepository expenseAccountRepository) : IExpenseAccountServices
    {
        public async Task<IEnumerable<ExpenseAccountViewModel>> GetAllExpenseAccounts()
        {
            var expenseAccounts = await expenseAccountRepository.GetAllAsync();
            return expenseAccounts.Select(ExpenseAccountViewModel.FromEntity);
        }

        public async Task<ExpenseAccountViewModel> GetExpenseAccountById(string id)
        {
            var expenseAccount = await expenseAccountRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense Account not found!");
            return ExpenseAccountViewModel.FromEntity(expenseAccount);
        }

        public async Task<ExpenseAccountViewModel> GetExpenseAccountByCode(string code)
        {
            var expenseAccount = await expenseAccountRepository.GetByCodeAsync(code) ?? throw new NotFoundException("Expense Account not found!");
            return ExpenseAccountViewModel.FromEntity(expenseAccount);
        }

        public async Task<ExpenseAccountViewModel> AddExpenseAccount(AddExpenseAccountInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create expense account!", errorsInput);
            }

            var expenseAccountExists = await expenseAccountRepository.GetByCodeAsync(inputModel.Code!);

            if (expenseAccountExists != null)
            {
                throw new BadRequestException("Expense Account already exists!", []);
            }

            var expenseAccount = inputModel.ToEntity();

            var expenseAccountCreated = await expenseAccountRepository.AddAsync(expenseAccount);

            return ExpenseAccountViewModel.FromEntity(expenseAccountCreated);
        }

        public async Task<ExpenseAccountViewModel> UpdateExpenseAccount(string id, ChangeExpenseAccountInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on update expense account!", errorsInput);
            }

            var expenseAccount = await expenseAccountRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense Account not found!");
            var codeExists = await expenseAccountRepository.GetByCodeAsync(inputModel.Code!);

            if (codeExists != null && codeExists.Id != id)
            {
                throw new BadRequestException("Code already exists!", []);
            }

            expenseAccount.Update(inputModel.Name!, inputModel.Code!, inputModel.Description!, AccountTypeExtensions.ToEnum(inputModel.Type.ToString()!));

            await expenseAccountRepository.UpdateAsync(expenseAccount);

            return ExpenseAccountViewModel.FromEntity(expenseAccount);
        }

        public async Task ActivateExpenseAccount(string id)
        {
            var expenseAccount = await expenseAccountRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense Account not found!");

            expenseAccount.Activate();

            await expenseAccountRepository.UpdateAsync(expenseAccount);
        }

        public async Task DeactivateExpenseAccount(string id)
        {
            var expenseAccount = await expenseAccountRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense Account not found!");

            expenseAccount.Deactivate();

            await expenseAccountRepository.UpdateAsync(expenseAccount);
        }
    }
}