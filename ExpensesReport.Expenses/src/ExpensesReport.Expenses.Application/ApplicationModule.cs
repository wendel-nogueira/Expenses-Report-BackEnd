using ExpensesReport.Expenses.Application.Services.Expense;
using ExpensesReport.Expenses.Application.Services.ExpenseAccount;
using ExpensesReport.Expenses.Application.Services.ExpenseReport;
using ExpensesReport.Expenses.Application.Services.Signature;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Expenses.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationServices();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddSingleton(configuration!);
        services.AddScoped<IExpenseServices, ExpenseServices>();
        services.AddScoped<IExpenseAccountServices, ExpenseAccountServices>();
        services.AddScoped<IExpenseReportServices, ExpenseReportServices>();
        services.AddScoped<ISignatureServices, SignatureServices>();

        return services;
    }
}
