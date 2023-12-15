using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Repositories;

namespace ExpensesReport.Expenses.Application.Services.ExpenseReport
{
    public class ExpenseReportServices(IExpenseReportRepository expenseReportRepository, IExpenseRepository expenseRepository, ISignatureRepository signatureRepository) : IExpenseReportServices
    {
        public async Task<IEnumerable<ExpenseReportViewModel>> GetAllExpenseReports()
        {
            var expenseReports = await expenseReportRepository.GetAllAsync();
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<ExpenseReportViewModel> GetExpenseReportById(string id)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");

            var expenses = await expenseRepository.GetAllInExpenseReportAsync(id);
            if (expenses != null)
                expenseReport.Expenses = (ICollection<Core.Entities.Expense>)expenses;

            var signatures = await signatureRepository.GetAllInExpenseReportAsync(id);
            if (signatures != null)
                expenseReport.Signatures = (ICollection<Core.Entities.Signature>)signatures;

            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByUser(Guid userId)
        {
            var expenseReports = await expenseReportRepository.GetByUserAsync(userId);
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByDepartament(Guid departamentId)
        {
            var expenseReports = await expenseReportRepository.GetByDepartamentAsync(departamentId);
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByProject(Guid projectId)
        {
            var expenseReports = await expenseReportRepository.GetByProjectAsync(projectId);
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<ExpenseReportViewModel> AddExpenseReport(AddExpenseReportInputModel inputModel)
        {
            var errorsInput = InputModelValidator.Validate(inputModel);

            if (errorsInput?.Length > 0)
            {
                throw new BadRequestException("Error on create expense report!", errorsInput);
            }

            var expenseReport = inputModel.ToEntity();
            await expenseReportRepository.AddAsync(expenseReport);

            if (inputModel.Expenses != null)
            {
                foreach (var expense in inputModel.Expenses)
                {
                    await expenseRepository.AddAsync(expenseReport.Id.ToString(), expense.ToEntity());
                }
            }

            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task<ExpenseReportViewModel> UpdateExpenseReport(string id, ChangeExpenseReportInputModel inputModel)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");

            expenseReport.Update(
                expenseReport.UserId,
                expenseReport.DepartamentId,
                expenseReport.ProjectId,
                inputModel.Status!.Value,
                inputModel.TotalAmount!.Value,
                inputModel.AmountApproved!.Value,
                inputModel.AmountRejected!.Value,
                inputModel.AmountPaid!.Value,
                inputModel.PaidById!.Value,
                inputModel.PaidDate!.Value,
                inputModel.StatusNotes!,
                inputModel.ProofOfPayment!,
                inputModel.PaidDateTimeZone!);

            await expenseReportRepository.UpdateAsync(expenseReport);

            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task ActivateExpenseReport(string id)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");

            expenseReport.Activate();
            await expenseReportRepository.UpdateAsync(expenseReport);
        }

        public async Task DeactivateExpenseReport(string id)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");

            expenseReport.Deactivate();
            await expenseReportRepository.UpdateAsync(expenseReport);
        }
    }
}
