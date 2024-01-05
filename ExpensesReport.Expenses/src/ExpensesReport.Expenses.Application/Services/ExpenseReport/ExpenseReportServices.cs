using ExpensesReport.Expenses.Application.Exceptions;
using ExpensesReport.Expenses.Application.InputModels.ExpenseReportInputModel;
using ExpensesReport.Expenses.Application.Publishers;
using ExpensesReport.Expenses.Application.Validators;
using ExpensesReport.Expenses.Application.ViewModels;
using ExpensesReport.Expenses.Core.Enums;
using ExpensesReport.Expenses.Core.Repositories;
using ExpensesReport.Expenses.Infrastructure.Queue;
using Microsoft.Extensions.Configuration;

namespace ExpensesReport.Expenses.Application.Services.ExpenseReport
{
    public class ExpenseReportServices : IExpenseReportServices
    {
        public string? Token { get; set; }
        private string? ApiGatewayUrl { get; set; }
        private string? ApplicationUri { get; set; }

        private readonly IExpenseReportRepository expenseReportRepository;
        private readonly IExpenseRepository expenseRepository;
        private readonly ISignatureRepository signatureRepository;
        private readonly MailQueue mailQueue;

        public ExpenseReportServices(IExpenseReportRepository expenseReportRepository, IExpenseRepository expenseRepository, ISignatureRepository signatureRepository, MailQueue mailQueue, IConfiguration config)
        {
            this.expenseReportRepository = expenseReportRepository;
            this.expenseRepository = expenseRepository;
            this.signatureRepository = signatureRepository;
            this.mailQueue = mailQueue;

            ApiGatewayUrl = config.GetSection("ApiGatewayUrl").Value ?? Environment.GetEnvironmentVariable("ApiGatewayUrl");
            ApplicationUri = config.GetSection("ApplicationUri").Value ?? Environment.GetEnvironmentVariable("ApplicationUri");
        }

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

            var totalAmount = inputModel.Expenses?.Sum(x => x.Amount) ?? 0;
            var expenseReport = inputModel.ToEntity();
            expenseReport.TotalAmount = totalAmount;

            if (inputModel.Expenses != null)
            {
                foreach (var expense in inputModel.Expenses)
                {
                    expenseReport.Expenses.Add(expense.ToEntity());
                }
            }

            await expenseReportRepository.AddAsync(expenseReport);

            var endpointUser = $"{ApiGatewayUrl}/api/users/{expenseReport.UserId}/supervisors";
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var response = await client.GetAsync(endpointUser);
            var publisher = new MailPublisher(mailQueue);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var supervisors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(result) ?? throw new NotFoundException("User does not have supervisors!");

                foreach (var supervisor in supervisors)
                {
                    var endpointIdentity = $"{ApiGatewayUrl}/api/identity/{supervisor.IdentityId}";
                    var responseIdentity = await client.GetAsync(endpointIdentity);

                    if (responseIdentity.IsSuccessStatusCode)
                    {
                        var resultIdentity = await responseIdentity.Content.ReadAsStringAsync();
                        var identity = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityViewModel>(resultIdentity);

                        if (identity != null)
                        {
                            publisher.SendMail(
                                identity.Email,
                                $"New expense report - [{expenseReport.Id}]",
                                $"A new expense report was created!",
                                $"{supervisor.Name.FirstName} {supervisor.Name.LastName}",
                                "A new expense report has been generated and is awaiting your review.",
                                true,
                                "view expense report",
                                $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                );
                        }
                    }
                }
            }

            return ExpenseReportViewModel.FromEntity(expenseReport);
        }

        public async Task<ExpenseReportViewModel> UpdateExpenseReport(string id, ChangeExpenseReportInputModel inputModel)
        {
            var expenseReport = await expenseReportRepository.GetByIdAsync(id) ?? throw new NotFoundException("Expense report not found!");
            var oldStatus = expenseReport.Status;

            expenseReport.Update(
                (Guid)inputModel.DepartamentId!,
                (Guid)inputModel.ProjectId!,
                inputModel.Status!.Value,
                inputModel.AmountPaid!,
                inputModel.PaidById!,
                inputModel.PaidDate!,
                inputModel.StatusNotes!,
                inputModel.ProofOfPayment!,
                inputModel.PaidDateTimeZone!);

            await expenseReportRepository.UpdateAsync(expenseReport);

            var enpointUser = $"{ApiGatewayUrl}/api/users";
            var endpointIdentity = $"{ApiGatewayUrl}/api/identity";
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var publisher = new MailPublisher(mailQueue);

            if (oldStatus != expenseReport.Status)
                switch (expenseReport.Status)
                {
                    case ExpenseReportStatus.ApprovedBySupervisor:
                        {
                            var responseUser = await client.GetAsync($"{enpointUser}/{expenseReport.UserId}");

                            if (responseUser.IsSuccessStatusCode)
                            {
                                var resultUser = await responseUser.Content.ReadAsStringAsync();
                                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(resultUser);

                                if (user != null)
                                {
                                    var responseIdentity = await client.GetAsync($"{endpointIdentity}/{user.IdentityId}");

                                    if (responseIdentity.IsSuccessStatusCode)
                                    {
                                        var resultIdentity = await responseIdentity.Content.ReadAsStringAsync();
                                        var identity = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityViewModel>(resultIdentity);

                                        if (identity != null)
                                        {
                                            publisher.SendMail(
                                                identity.Email,
                                                $"Expense report approved - [{expenseReport.Id}]",
                                                $"Your Expense Report Has Been Approved!",
                                                $"{user.Name.FirstName} {user.Name.LastName}",
                                                $"Your expense report has been approved by the manager. It will now be forwarded to the accountant for payment processing. \n\nNotes: {expenseReport.StatusNotes}",
                                                true,
                                                "view expense report",
                                                $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                                );
                                        }
                                    }
                                }
                            }

                            var responseAccountants = await client.GetAsync($"{endpointIdentity}/role/Accountant");

                            if (responseAccountants.IsSuccessStatusCode)
                            {
                                var resultAccountants = await responseAccountants.Content.ReadAsStringAsync();
                                var accountants = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<IdentityViewModel>>(resultAccountants);

                                if (accountants != null)
                                {
                                    foreach (var accountant in accountants)
                                    {
                                        var accountantUser = await client.GetAsync($"{enpointUser}/identity/{accountant.Id}");

                                        if (accountantUser.IsSuccessStatusCode)
                                        {
                                            var resultAccountantUser = await accountantUser.Content.ReadAsStringAsync();
                                            var accountantUserViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(resultAccountantUser);

                                            if (accountantUserViewModel != null)
                                            {
                                                publisher.SendMail(
                                                    accountant.Email,
                                                    $"New expense report - [{expenseReport.Id}]",
                                                    $"A new expense report was approved!",
                                                    $"{accountantUserViewModel.Name.FirstName} {accountantUserViewModel.Name.LastName}",
                                                    $"A new expense report has been generated and is awaiting your review. \n\nNotes: {expenseReport.StatusNotes}",
                                                    true,
                                                    "view expense report",
                                                    $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ExpenseReportStatus.RejectedBySupervisor:
                        {
                            var responseUser = await client.GetAsync($"{enpointUser}/{expenseReport.UserId}");

                            if (responseUser.IsSuccessStatusCode)
                            {
                                var resultUser = await responseUser.Content.ReadAsStringAsync();
                                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(resultUser);

                                if (user != null)
                                {
                                    var responseIdentity = await client.GetAsync($"{endpointIdentity}/{user.IdentityId}");

                                    if (responseIdentity.IsSuccessStatusCode)
                                    {
                                        var resultIdentity = await responseIdentity.Content.ReadAsStringAsync();
                                        var identity = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityViewModel>(resultIdentity);

                                        if (identity != null)
                                        {
                                            publisher.SendMail(
                                                identity.Email,
                                                $"Expense report rejected - [{expenseReport.Id}]",
                                                $"Your Expense Report Has Been Rejected!",
                                                $"{user.Name.FirstName} {user.Name.LastName}",
                                                $"Unfortunately, your expense report has been disapproved by the manager. Please check the report and make the necessary changes. \n\nNotes: {expenseReport.StatusNotes}",
                                                true,
                                                "view expense report",
                                                $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                                );
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case ExpenseReportStatus.Paid or ExpenseReportStatus.PaymentRejected:
                        {
                            var responseUser = await client.GetAsync($"{enpointUser}/{expenseReport.UserId}");

                            if (responseUser.IsSuccessStatusCode)
                            {
                                var resultUser = await responseUser.Content.ReadAsStringAsync();
                                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(resultUser);

                                if (user != null)
                                {
                                    var responseIdentity = await client.GetAsync($"{endpointIdentity}/{user.IdentityId}");

                                    if (responseIdentity.IsSuccessStatusCode)
                                    {
                                        var resultIdentity = await responseIdentity.Content.ReadAsStringAsync();
                                        var identity = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityViewModel>(resultIdentity);

                                        if (identity != null)
                                        {
                                            publisher.SendMail(
                                                identity.Email,
                                                $"Expense report {expenseReport.Status switch { ExpenseReportStatus.Paid => "paid", ExpenseReportStatus.PaymentRejected => "payment rejected", _ => "" }} - [{expenseReport.Id}]",
                                                $"Your Expense Report Has Been {expenseReport.Status switch { ExpenseReportStatus.Paid => "Paid", ExpenseReportStatus.PaymentRejected => "Payment Rejected", _ => "" }}!",
                                                $"{user.Name.FirstName} {user.Name.LastName}",
                                                $"Your expense report has been {expenseReport.Status switch { ExpenseReportStatus.Paid => "paid", ExpenseReportStatus.PaymentRejected => "payment rejected", _ => "" }} by the accountant. \n\nNotes: {expenseReport.StatusNotes}",
                                                true,
                                                "view expense report",
                                                $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                                );
                                        }

                                        var responseSupervisors = await client.GetAsync($"{enpointUser}/{expenseReport.UserId}/supervisors");

                                        if (responseSupervisors.IsSuccessStatusCode)
                                        {
                                            var resultSupervisors = await responseSupervisors.Content.ReadAsStringAsync();
                                            var supervisors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(resultSupervisors);

                                            if (supervisors != null)
                                            {
                                                foreach (var supervisor in supervisors)
                                                {
                                                    var supervisorIdentity = await client.GetAsync($"{endpointIdentity}/{supervisor.IdentityId}");

                                                    if (supervisorIdentity.IsSuccessStatusCode)
                                                    {
                                                        var resultSupervisorIdentity = await supervisorIdentity.Content.ReadAsStringAsync();
                                                        var supervisorIdentityViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityViewModel>(resultSupervisorIdentity);

                                                        if (supervisorIdentityViewModel != null)
                                                        {
                                                            publisher.SendMail(
                                                                supervisorIdentityViewModel.Email,
                                                                $"Expense report {expenseReport.Status switch { ExpenseReportStatus.Paid => "paid", ExpenseReportStatus.PaymentRejected => "payment rejected", _ => "" }} - [{expenseReport.Id}]",
                                                                $"Expense Report Has Been {expenseReport.Status switch { ExpenseReportStatus.Paid => "Paid", ExpenseReportStatus.PaymentRejected => "Payment Rejected", _ => "" }}!",
                                                                $"{supervisor.Name.FirstName} {supervisor.Name.LastName}",
                                                                $"The expense report [{expenseReport.Id}] has been {expenseReport.Status switch { ExpenseReportStatus.Paid => "paid", ExpenseReportStatus.PaymentRejected => "payment rejected", _ => "" }} by the accountant. \n\nNotes: {expenseReport.StatusNotes}",
                                                                true,
                                                                "view expense report",
                                                                $"{ApplicationUri}/expense-reports/edit/{expenseReport.Id}"
                                                                );
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                }

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
