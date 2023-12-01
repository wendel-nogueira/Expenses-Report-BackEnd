using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Repositories;

namespace ExpensesReport.Expenses.Application.Services.ExpenseReport
{
    public class ExpenseReportServices(IExpenseReportRepository expenseReportRepository) : IExpenseReportServices
    {
        public async Task<IEnumerable<ExpenseReportViewModel>> GetAllExpenseReports()
        {
            var expenseReports = await expenseReportRepository.GetAllAsync();
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<ExpenseReportViewModel> GetExpenseReportById(string id)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByUser(Guid userId)
        {
            var expenseReports = await expenseReportRepository.GetByUserAsync(userId);
            return expenseReports.Select(ExpenseReportViewModel.FromEntity);
        }

        public async Task<IEnumerable<ExpenseReportViewModel>> GetExpenseReportsByDepartment(Guid departmentId)
        {
            var expenseReports = await expenseReportRepository.GetByDepartmentAsync(departmentId);
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

            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task<ExpenseReportViewModel> UpdateExpenseReport(string id, ChangeExpenseReportInputModel inputModel)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");

            expenseReport.Update(
                expenseReport.UserId,
                expenseReport.DepartmentId,
                expenseReport.ProjectId,
                inputModel.Status!.Value,
                inputModel.TotalAmount!.Value,
                inputModel.AmountApproved!.Value,
                inputModel.AmountRejected!.Value,
                inputModel.AmountPaid!.Value,
                inputModel.PaidById!.Value,
                inputModel.PaidDate!.Value,
                inputModel.StatusNotes!,
                inputModel.ProofOfPayment!);

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
