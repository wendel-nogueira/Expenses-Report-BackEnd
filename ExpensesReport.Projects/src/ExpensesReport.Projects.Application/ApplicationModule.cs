using ExpensesReport.Projects.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Projects.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationServices();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectServices, ProjectServices>();
            return services;
        }
    }
}
