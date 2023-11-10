using ExpensesReport.Files.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExpensesReport.Files.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationServices();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        var storageAccount = configuration.GetSection("Azure:StorageAccount").Value;
        if (string.IsNullOrEmpty(storageAccount)) storageAccount = Environment.GetEnvironmentVariable("StorageAccount");

        var key = configuration.GetSection("Azure:Key").Value;
        if (string.IsNullOrEmpty(key)) key = Environment.GetEnvironmentVariable("Key");

        var container = configuration.GetSection("Azure:Container").Value;
        if (string.IsNullOrEmpty(container)) container = Environment.GetEnvironmentVariable("Container");

        if (string.IsNullOrEmpty(storageAccount) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(container))
            throw new ArgumentNullException("Azure configuration is missing");

        services.AddScoped<IFilesServices>(x => new FilesServices(storageAccount, key, container));

        return services;
    }
}
