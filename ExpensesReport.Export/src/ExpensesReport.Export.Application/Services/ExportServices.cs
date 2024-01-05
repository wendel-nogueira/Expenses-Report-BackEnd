using ExpensesReport.Export.Application.ViewModels;
using ExpensesReport.Export.Application.Exceptions;
using Newtonsoft.Json;
using ExpensesReport.Export.Core.Entities;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPDF.Helpers;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace ExpensesReport.Export.Application.Services
{
    public class ExportServices : IExportServices
    {
        public string? Token { get; set; }
        private string? ApiGatewayUrl { get; set; }
        private readonly BlobContainerClient _containerClient;

        public ExportServices(string storageAccount, string key, string container, IConfiguration config)
        {
            var credential = new StorageSharedKeyCredential(storageAccount, key);
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccount}.blob.core.windows.net"), credential);
            _containerClient = blobServiceClient.GetBlobContainerClient(container);

            ApiGatewayUrl = config.GetSection("ApiGatewayUrl").Value ?? Environment.GetEnvironmentVariable("ApiGatewayUrl");

            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<FileViewModel> exportUsersAndIdentites()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseIdentities = await client.GetAsync($"{ApiGatewayUrl}/api/identity");
            var responseUsers = await client.GetAsync($"{ApiGatewayUrl}/api/users");

            if (responseIdentities.IsSuccessStatusCode && responseUsers.IsSuccessStatusCode)
            {
                var resultIdentities = await responseIdentities.Content.ReadAsStringAsync();
                var resultUsers = await responseUsers.Content.ReadAsStringAsync();

                if (resultIdentities != null && resultUsers != null)
                {
                    var identities = JsonConvert.DeserializeObject<List<Identity>>(resultIdentities);
                    var users = JsonConvert.DeserializeObject<List<User>>(resultUsers);

                    if ((identities == null || users == null) || (identities.Count == 0 || users.Count == 0))
                        throw new NotFoundException("No users or identities found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    column.Item().Text("Users and Identities")
                                        .SemiBold()
                                        .FontSize(12)
                                        .FontColor(Colors.Grey.Darken2);

                                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn();
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Name")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Email")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Role")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Address")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Active")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        foreach (var identity in identities)
                                        {
                                            var index = identities.IndexOf(identity) + 1;
                                            var name = users.Find(x => x.IdentityId == identity.Id)?.Name?.ToString() ?? "N/A";
                                            var address = users.Find(x => x.IdentityId == identity.Id)?.Address?.ToString() ?? "N/A";
                                            var active = identity.IsDeleted == false ? "Yes" : "No";

                                            table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{name}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{identity.Email}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{identity.RoleName}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{address}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{active}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"usersAndIdentities_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
                else
                {
                    throw new NotFoundException("No users or identities found");
                }
            }

            throw new NotFoundException("No users or identities found");
        }

        public async Task<FileViewModel> exportDepartments()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseDepartments = await client.GetAsync($"{ApiGatewayUrl}/api/departaments");

            if (responseDepartments.IsSuccessStatusCode)
            {
                var resultDepartments = await responseDepartments.Content.ReadAsStringAsync();

                if (resultDepartments != null)
                {
                    var departments = JsonConvert.DeserializeObject<List<Department>>(resultDepartments);

                    if (departments == null || departments.Count == 0)
                        throw new NotFoundException("No departments found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    column.Item().Text("Departments")
                                        .SemiBold()
                                        .FontSize(12)
                                        .FontColor(Colors.Grey.Darken2);

                                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(4);
                                                columns.RelativeColumn();
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Name")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Acronym")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Description")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Active")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        foreach (var department in departments)
                                        {
                                            var index = departments.IndexOf(department) + 1;
                                            var active = department.IsDeleted == false ? "Yes" : "No";

                                            table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{department.Name}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{department.Acronym}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{department.Description}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{active}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"departments_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
            }

            throw new NotFoundException("No departments found");
        }

        public async Task<FileViewModel> exportProjects()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseDepartments = await client.GetAsync($"{ApiGatewayUrl}/api/departaments");
            var responseProjects = await client.GetAsync($"{ApiGatewayUrl}/api/projects");

            if (responseDepartments.IsSuccessStatusCode && responseProjects.IsSuccessStatusCode)
            {
                var resultDepartments = await responseDepartments.Content.ReadAsStringAsync();
                var resultProjects = await responseProjects.Content.ReadAsStringAsync();

                if (resultDepartments != null && resultProjects != null)
                {
                    var departments = JsonConvert.DeserializeObject<List<Department>>(resultDepartments);
                    var projects = JsonConvert.DeserializeObject<List<Project>>(resultProjects);

                    if ((departments == null || projects == null) || (departments.Count == 0 || projects.Count == 0))
                        throw new NotFoundException("No departments or projects found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    column.Item().Text("Projects")
                                        .SemiBold()
                                        .FontSize(12)
                                        .FontColor(Colors.Grey.Darken2);

                                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(4);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn();
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Name")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Code")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Description")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Department")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Active")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        foreach (var project in projects)
                                        {
                                            var index = projects.IndexOf(project) + 1;
                                            var active = project.IsDeleted == false ? "Yes" : "No";
                                            var department = departments.Find(x => x.Id == project.DepartamentId)?.Name?.ToString() ?? "N/A";

                                            table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{project.Name}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{project.Code}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{project.Description}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{department}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{active}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"projects_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
            }

            throw new NotFoundException("No departments or projects found");
        }

        public async Task<FileViewModel> exportExpenseAccounts()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseExpenseAccounts = await client.GetAsync($"{ApiGatewayUrl}/api/expense-accounts");

            if (responseExpenseAccounts.IsSuccessStatusCode)
            {
                var resultExpenseAccounts = await responseExpenseAccounts.Content.ReadAsStringAsync();

                if (resultExpenseAccounts != null)
                {
                    var expenseAccounts = JsonConvert.DeserializeObject<List<ExpenseAccount>>(resultExpenseAccounts);

                    if (expenseAccounts == null || expenseAccounts.Count == 0)
                        throw new NotFoundException("No expense accounts found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    column.Item().Text("Expense Accounts")
                                        .SemiBold()
                                        .FontSize(12)
                                        .FontColor(Colors.Grey.Darken2);

                                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(4);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn();
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Name")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Code")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Description")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Type")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Active")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        foreach (var expenseAccount in expenseAccounts)
                                        {
                                            var index = expenseAccounts.IndexOf(expenseAccount) + 1;
                                            var active = expenseAccount.IsDeleted == false ? "Yes" : "No";
                                            var type = expenseAccount.Type == 1 ? "Expense" : "Asset";

                                            table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{expenseAccount.Name}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{expenseAccount.Code}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{expenseAccount.Description}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{type}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{active}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"expenseAccounts_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
            }

            throw new NotFoundException("No expense accounts found");
        }

        public async Task<FileViewModel> exportExpenseReports()
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseDepartments = await client.GetAsync($"{ApiGatewayUrl}/api/departaments");
            var responseProjects = await client.GetAsync($"{ApiGatewayUrl}/api/projects");
            var responseUsers = await client.GetAsync($"{ApiGatewayUrl}/api/users");
            var responseExpenseReports = await client.GetAsync($"{ApiGatewayUrl}/api/expensereport");
            var responseExpenseAccounts = await client.GetAsync($"{ApiGatewayUrl}/api/expenseaccount");

            if (
                responseDepartments.IsSuccessStatusCode &&
                responseProjects.IsSuccessStatusCode &&
                responseUsers.IsSuccessStatusCode &&
                responseExpenseReports.IsSuccessStatusCode &&
                responseExpenseAccounts.IsSuccessStatusCode
            )
            {
                var resultDepartments = await responseDepartments.Content.ReadAsStringAsync();
                var resultProjects = await responseProjects.Content.ReadAsStringAsync();
                var resultUsers = await responseUsers.Content.ReadAsStringAsync();
                var resultExpenseAccounts = await responseExpenseAccounts.Content.ReadAsStringAsync();
                var resultExpenseReports = await responseExpenseReports.Content.ReadAsStringAsync();

                if (
                    resultDepartments != null &&
                    resultProjects != null &&
                    resultUsers != null &&
                    resultExpenseAccounts != null &&
                    resultExpenseReports != null
                )
                {
                    var departments = JsonConvert.DeserializeObject<List<Department>>(resultDepartments);
                    var projects = JsonConvert.DeserializeObject<List<Project>>(resultProjects);
                    var users = JsonConvert.DeserializeObject<List<User>>(resultUsers);
                    var expenseAccounts = JsonConvert.DeserializeObject<List<ExpenseAccount>>(resultExpenseAccounts);
                    var expenseReports = JsonConvert.DeserializeObject<List<ExpenseReport>>(resultExpenseReports);

                    if (
                        (departments == null || projects == null || users == null || expenseAccounts == null || expenseReports == null) ||
                        (departments.Count == 0 || projects.Count == 0 || users.Count == 0 || expenseAccounts.Count == 0 || expenseReports.Count == 0)
                        )
                        throw new NotFoundException("No expense accounts, departments, projects, users or expense reports found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    column.Item().Text("Expense Reports")
                                        .SemiBold()
                                        .FontSize(12)
                                        .FontColor(Colors.Grey.Darken2);

                                    column.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten1);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn();
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("User")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Department")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Project")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Status")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Total")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        foreach (var expenseReport in expenseReports)
                                        {
                                            var index = expenseReports.IndexOf(expenseReport) + 1;
                                            var active = expenseReport.IsDeleted == false ? "Yes" : "No";
                                            var user = users.Find(x => x.Id == expenseReport.UserId)?.Name?.ToString() ?? "N/A";
                                            var department = departments.Find(x => x.Id == expenseReport.DepartamentId)?.Name?.ToString() ?? "N/A";
                                            var project = projects.Find(x => x.Id == expenseReport.ProjectId)?.Name?.ToString() ?? "N/A";
                                            var status = expenseReport.Status switch
                                            {
                                                0 => "Submitted",
                                                1 => "Approved by Supervisor",
                                                2 => "Rejected by Supervisor",
                                                3 => "Paid",
                                                4 => "Payment Rejected",
                                                _ => "N/A"
                                            };

                                            var totalAmount = expenseReport.TotalAmount?.ToString("F2");

                                            table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{user}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{department}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{project}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"{status}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            table.Cell().Element(CellStyle).Text($"$ {totalAmount}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"expenseReports_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
            }

            throw new NotFoundException("No expense accounts, departments, projects, users or expense reports found");
        }

        public async Task<FileViewModel> exportExpenseReport(string id)
        {
            var client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            if (Token != null)
                headers.Add("Authorization", $"Bearer {Token}");

            var responseDepartments = await client.GetAsync($"{ApiGatewayUrl}/api/departaments");
            var responseProjects = await client.GetAsync($"{ApiGatewayUrl}/api/projects");
            var responseUsers = await client.GetAsync($"{ApiGatewayUrl}/api/users");
            var responseExpenseReport = await client.GetAsync($"{ApiGatewayUrl}/api/expensereport/{id}");
            var responseExpenseAccounts = await client.GetAsync($"{ApiGatewayUrl}/api/expenseaccount");

            if (
                responseDepartments.IsSuccessStatusCode &&
                responseProjects.IsSuccessStatusCode &&
                responseUsers.IsSuccessStatusCode &&
                responseExpenseReport.IsSuccessStatusCode &&
                responseExpenseAccounts.IsSuccessStatusCode
            )
            {
                var resultDepartments = await responseDepartments.Content.ReadAsStringAsync();
                var resultProjects = await responseProjects.Content.ReadAsStringAsync();
                var resultUsers = await responseUsers.Content.ReadAsStringAsync();
                var resultExpenseAccounts = await responseExpenseAccounts.Content.ReadAsStringAsync();
                var resultExpenseReport = await responseExpenseReport.Content.ReadAsStringAsync();

                if (
                    resultDepartments != null &&
                    resultProjects != null &&
                    resultUsers != null &&
                    resultExpenseAccounts != null &&
                    resultExpenseReport != null
                )
                {
                    var departments = JsonConvert.DeserializeObject<List<Department>>(resultDepartments);
                    var projects = JsonConvert.DeserializeObject<List<Project>>(resultProjects);
                    var users = JsonConvert.DeserializeObject<List<User>>(resultUsers);
                    var expenseAccounts = JsonConvert.DeserializeObject<List<ExpenseAccount>>(resultExpenseAccounts);
                    var expenseReport = JsonConvert.DeserializeObject<ExpenseReport>(resultExpenseReport);

                    if (
                        (departments == null || projects == null || users == null || expenseAccounts == null || expenseReport == null) ||
                        (departments.Count == 0 || projects.Count == 0 || users.Count == 0 || expenseAccounts.Count == 0)
                        )
                        throw new NotFoundException("No expense accounts, departments, projects, users or expense report found");

                    var pdfStream = new MemoryStream();

                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            const float horizontalMargin = 0.4f;
                            const float verticalMargin = 0.6f;

                            page.Size(PageSizes.A4);
                            page.MarginVertical(verticalMargin, Unit.Inch);
                            page.MarginHorizontal(horizontalMargin, Unit.Inch);

                            //page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));

                            page.Header()
                                .Height(80)
                                .AlignCenter()
                                .Element(Header);

                            page.Content()
                                .Column(column =>
                                {
                                    var active = expenseReport.IsDeleted == false ? "Yes" : "No";
                                    var user = users.Find(x => x.Id == expenseReport.UserId)?.Name?.ToString() ?? "N/A";
                                    var department = departments.Find(x => x.Id == expenseReport.DepartamentId)?.Name?.ToString() ?? "N/A";
                                    var project = projects.Find(x => x.Id == expenseReport.ProjectId)?.Name?.ToString() ?? "N/A";
                                    var status = expenseReport.Status switch
                                    {
                                        0 => "Submitted",
                                        1 => "Approved by Supervisor",
                                        2 => "Rejected by Supervisor",
                                        3 => "Paid",
                                        4 => "Payment Rejected",
                                        _ => "N/A"
                                    };

                                    var totalAmount = expenseReport.TotalAmount?.ToString("F2");
                                    var totalApprovedAmount = expenseReport.AmountApproved?.ToString("F2");
                                    var totalRejectedAmount = expenseReport.AmountRejected?.ToString("F2");
                                    var totalPaidAmount = expenseReport.AmountPaid?.ToString("F2");

                                    column.Item()
                                        .Text(x =>
                                        {
                                            x.Span("Expense Report:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"#{expenseReport.Id}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingRight(160).PaddingLeft(92).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    column.Item()
                                        .PaddingTop(10)
                                        .Text(x =>
                                        {
                                            x.Span("Submitted by:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{user}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingRight(160).PaddingLeft(80).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    column.Item()
                                        .PaddingTop(10)
                                        .Text(x =>
                                        {
                                            x.Span("Department:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{department}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingRight(160).PaddingLeft(75).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    column.Item()
                                        .PaddingTop(10)
                                        .Text(x =>
                                        {
                                            x.Span("Project:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{project}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingRight(160).PaddingLeft(47).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    column.Item()
                                        .PaddingTop(10)
                                        .Text(x =>
                                        {
                                            x.Span("Status:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{status}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingRight(160).PaddingLeft(43).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    if (expenseReport.StatusNotes != null)
                                    {
                                        column.Item()
                                            .PaddingTop(10)
                                            .Text(x =>
                                            {
                                                x.Span("Status Notes:   ")
                                                    .Bold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken4);

                                                x.Span($"{expenseReport.StatusNotes}")
                                                    .SemiBold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken2);
                                            });
                                    }

                                    column.Item().PaddingRight(160).PaddingLeft(78).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                    column.Item().PaddingTop(40);

                                    column.Item()
                                        .PaddingTop(2)
                                        .Text("Expenses")
                                        .Bold()
                                        .FontSize(14)
                                        .FontColor(Colors.Grey.Darken4);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(25);
                                                columns.RelativeColumn(1.3f);
                                                columns.RelativeColumn(3);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(2);
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                                .Text("#")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Date")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Explanation")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Account")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Status")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Amount")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        if (expenseReport.Expenses != null)
                                        {
                                            var expenses = expenseReport.Expenses;

                                            foreach (var expense in expenses)
                                            {
                                                var index = expenses.ToList().IndexOf(expense) + 1;
                                                var account = expenseAccounts.Find(x => x.Id == expense.ExpenseAccount)?.Name?.ToString() ?? "N/A";
                                                var status = expense.Status switch
                                                {
                                                    0 => "Approved",
                                                    1 => "Rejected",
                                                    _ => "N/A"
                                                };

                                                var amount = expense.Amount?.ToString("F2");

                                                table.Cell().Element(CellStyle).Text($"{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{expense.DateIncurred?.ToString("dd/MM/yyyy")}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{expense.Explanation}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{account}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{status}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"$ {amount}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            }
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });

                                    column.Item()
                                        .AlignRight()
                                        .PaddingTop(8)
                                        .Text(x =>
                                        {
                                            x.Span("Total:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{totalAmount}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item()
                                        .AlignRight()
                                        .PaddingTop(4)
                                        .Text(x =>
                                        {
                                            x.Span("Total Approved:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{totalApprovedAmount}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item()
                                        .AlignRight()
                                        .PaddingTop(4)
                                        .Text(x =>
                                        {
                                            x.Span("Total Rejected:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{totalRejectedAmount}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item()
                                        .AlignRight()
                                        .PaddingTop(4)
                                        .Text(x =>
                                        {
                                            x.Span("Total Paid:   ")
                                                .Bold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken4);

                                            x.Span($"{totalPaidAmount}")
                                                .SemiBold()
                                                .FontSize(12)
                                                .FontColor(Colors.Grey.Darken2);
                                        });

                                    column.Item().PaddingTop(40);

                                    column.Item()
                                        .PaddingTop(2)
                                        .Text("Expenses Actions")
                                        .Bold()
                                        .FontSize(14)
                                        .FontColor(Colors.Grey.Darken4);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(0);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(1.3f);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(1.3f);
                                                columns.RelativeColumn(4);
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                            .Text("")
                                            .SemiBold()
                                            .FontSize(10)
                                            .FontColor(Colors.Grey.Darken2);

                                            header.Cell()
                                                .Text("Expense")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Status")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Action By")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Action Date")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Notes")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(6).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        if (expenseReport.Expenses != null)
                                        {
                                            var expenses = expenseReport.Expenses;

                                            foreach (var expense in expenses)
                                            {
                                                var index = expenses.ToList().IndexOf(expense) + 1;
                                                var status = expense.Status switch
                                                {
                                                    0 => "Approved",
                                                    1 => "Rejected",
                                                    _ => "N/A"
                                                };
                                                var actionBy = users.Find(x => x.Id == expense.ActionById)?.Name?.ToString() ?? "N/A";

                                                table.Cell().Element(CellStyle).Text($"").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"Expense  #{index}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{status}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{actionBy}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{expense.ActionDate?.ToString("dd/MM/yyyy")}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{expense.AccountingNotes}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            }
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });

                                    column.Item().PaddingTop(40);

                                    column.Item()
                                        .PaddingTop(2)
                                        .Text("Signatures")
                                        .Bold()
                                        .FontSize(14)
                                        .FontColor(Colors.Grey.Darken4);

                                    column.Item().PaddingTop(20);

                                    column.Item().Table(table =>
                                    {
                                        table
                                            .ColumnsDefinition(columns =>
                                            {
                                                columns.ConstantColumn(0);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(1.3f);
                                                columns.RelativeColumn(2);
                                                columns.RelativeColumn(1.3f);
                                            });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                            .Text("")
                                            .SemiBold()
                                            .FontSize(10)
                                            .FontColor(Colors.Grey.Darken2);

                                            header.Cell()
                                                .Text("Name")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Acceptance")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("Date")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().Text("IP Address")
                                                .SemiBold()
                                                .FontSize(10)
                                                .FontColor(Colors.Grey.Darken2);

                                            header.Cell().ColumnSpan(5).PaddingTop(5).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                        });

                                        if (expenseReport.Signatures != null)
                                        {
                                            var signatures = expenseReport.Signatures;

                                            foreach (var signature in signatures)
                                            {
                                                var index = signatures.ToList().IndexOf(signature) + 1;
                                                var acceptace = signature.Acceptance == true ? "Yes" : "No";

                                                table.Cell().Element(CellStyle).Text($"").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{signature.Name}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{acceptace}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{signature.SignatureDate?.ToString("dd/MM/yyyy")}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                                table.Cell().Element(CellStyle).Text($"{signature.IpAddress}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            }
                                        }

                                        static IContainer CellStyle(IContainer container) => container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(10);
                                    });

                                    if (expenseReport.Status == 3)
                                    {
                                        var paidBy = users.Find(x => x.Id == expenseReport.PaidById)?.Name?.ToString() ?? "N/A";

                                        column.Item().PaddingTop(30);

                                        column.Item()
                                            .PaddingTop(2)
                                            .Text("Payment")
                                            .Bold()
                                            .FontSize(14)
                                            .FontColor(Colors.Grey.Darken4);

                                        column.Item()
                                            .PaddingTop(16)
                                            .Text(x =>
                                            {
                                                x.Span("Paid by:   ")
                                                    .Bold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken4);

                                                x.Span($"{paidBy}")
                                                    .SemiBold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken2);
                                            });

                                        column.Item().PaddingRight(160).PaddingLeft(47).BorderBottom(1).BorderColor(Colors.Grey.Darken3);

                                        column.Item()
                                            .PaddingTop(10)
                                            .Text(x =>
                                            {
                                                x.Span("Paid date:   ")
                                                    .Bold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken4);

                                                x.Span($"{expenseReport.PaidDate?.ToString("dd/MM/yyyy")}")
                                                    .SemiBold()
                                                    .FontSize(12)
                                                    .FontColor(Colors.Grey.Darken2);
                                            });

                                        column.Item().PaddingRight(160).PaddingLeft(59).BorderBottom(1).BorderColor(Colors.Grey.Darken3);
                                    }
                                });

                            page.Footer()
                                .Height(15)
                                .AlignCenter()
                                .Element(Footer);
                        });
                    }).GeneratePdf(pdfStream);

                    pdfStream.Seek(0, SeekOrigin.Begin);

                    var fileName = $"expenseReport_{Guid.NewGuid()}.pdf";
                    var blobClient = _containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(pdfStream, true);

                    FileViewModel fileViewModel = new()
                    {
                        Name = fileName,
                        Uri = blobClient.Uri.AbsoluteUri
                    };

                    return fileViewModel;
                }
            }

            throw new NotFoundException("No expense accounts, departments, projects, users or expense report found");
        }

        void Header(IContainer container)
        {
            var relativeImagePath = Path.Combine("Images", "logo.png");
            var absoluteImagePath = Path.Combine(Directory.GetCurrentDirectory(), relativeImagePath);
            var logo = Image.FromFile(absoluteImagePath);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item()
                        .Text("Expenses Report")
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(Colors.Blue.Darken3);

                    column.Item()
                        .Text($"Exported at: {DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss")} - Timezone: {TimeZoneInfo.Local.DisplayName}")
                        .FontSize(12)
                        .FontColor(Colors.Grey.Darken2);
                });

                row.ConstantItem(80).Image(logo);
            });
        }

        void Footer(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem()
                .AlignRight()
                .Text(x =>
                {
                    x.Span("Page ")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken2);

                    x.CurrentPageNumber()
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken2);

                    x.Span(" of ")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken2);

                    x.TotalPages()
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken2);
                });
            });
        }
    }
}
