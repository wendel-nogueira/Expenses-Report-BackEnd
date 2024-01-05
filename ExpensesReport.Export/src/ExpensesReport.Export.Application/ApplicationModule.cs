using ExpensesReport.Export.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesReport.Export.Application
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
            var configurations = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            var storageAccount = configurations.GetSection("Azure:StorageAccount").Value;
            if (string.IsNullOrEmpty(storageAccount)) storageAccount = Environment.GetEnvironmentVariable("StorageAccount");

            var key = configurations.GetSection("Azure:Key").Value;
            if (string.IsNullOrEmpty(key)) key = Environment.GetEnvironmentVariable("Key");

            var container = configurations.GetSection("Azure:Container").Value;
            if (string.IsNullOrEmpty(container)) container = Environment.GetEnvironmentVariable("Container");

            if (string.IsNullOrEmpty(storageAccount) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(container))
                throw new ArgumentNullException("Azure configuration is missing");

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddSingleton(configuration!);
            services.AddScoped<IExportServices>(x => new ExportServices(storageAccount, key, container, configuration!));

            return services;
        }
    }
}
