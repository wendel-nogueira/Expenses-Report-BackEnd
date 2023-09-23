using ExpensesReport.Users.Core.Repositories;
using ExpensesReport.Users.Infrastructure.Persistence;
using ExpensesReport.Users.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Users.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPersistence()
            .AddRepositories();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .AddSingleton<UsersDbContext>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

}
