using ExpensesReport.Users.Core.Repositories;
using ExpensesReport.Users.Infrastructure.Persistence;
using ExpensesReport.Users.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

}
