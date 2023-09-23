using ExpensesReport.Users.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Users.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationServices();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserServices, UserServices>();
        return services;
    }
}
