using ExpensesReport.Identity.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Identity.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationServices();
            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddSingleton(configuration!);
            services.AddScoped<IIdentityServices, IdentityServices>();
            return services;
        }
    }
}
