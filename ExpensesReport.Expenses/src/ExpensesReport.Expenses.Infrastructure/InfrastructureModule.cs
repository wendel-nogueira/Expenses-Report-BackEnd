using ExpensesReport.Expenses.Core.Repositories;
using ExpensesReport.Expenses.Infrastructure.Persistence.Context;
using ExpensesReport.Expenses.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace ExpensesReport.Expenses.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPersistence()
            .AddRepositories()
            .AddAuthentication();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<IExpensesDbContext>(provider =>
        {
            var configuration = provider.GetService<IConfiguration>();

            var connectionString = configuration!.GetSection("MongoDb:ConnectionString").Value!;
            if (string.IsNullOrEmpty(connectionString)) connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            var databaseName = configuration.GetSection("MongoDb:DatabaseName").Value!;
            if (string.IsNullOrEmpty(databaseName)) databaseName = Environment.GetEnvironmentVariable("DB_NAME");

            return new ExpensesDbContext(connectionString!, databaseName!);
        });

        services.AddSingleton<IMongoClient>(provider =>
        {
            var expensesDbContext = provider.GetService<IExpensesDbContext>();

            return new MongoClient(expensesDbContext!.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(provider =>
        {
            var expensesDbContext = provider.GetService<IExpensesDbContext>();
            var mongoClient = provider.GetService<IMongoClient>();

            return mongoClient!.GetDatabase(expensesDbContext!.Database);
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IExpenseAccountRepository, ExpenseAccountRepository>();
        services.AddScoped<IExpenseReportRepository, ExpenseReportRepository>();
        services.AddScoped<ISignatureRepository, SignatureRepository>();

        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration!.GetSection("Jwt:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value!)),
                ValidateIssuerSigningKey = true
            };
        });

        return services;
    }
}
