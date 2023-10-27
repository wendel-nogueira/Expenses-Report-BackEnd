using ExpensesReport.Departaments.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Departaments.Application
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
            services.AddScoped<IDepartamentServices, DepartamentServices>();
            return services;
        }
    }
}
