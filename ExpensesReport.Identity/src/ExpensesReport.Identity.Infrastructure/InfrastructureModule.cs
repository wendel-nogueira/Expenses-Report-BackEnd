using Azure.Messaging.ServiceBus;
using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.Repositories;
using ExpensesReport.Identity.Infrastructure.Persistence.Context;
using ExpensesReport.Identity.Infrastructure.Persistence.Repositories;
using ExpensesReport.Identity.Infrastructure.Queue;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpensesReport.Identity.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddPersistence()
                .AddRepositories()
                .AddIdentity()
                .AddQueue();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<UserIdentityDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
                var dbName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
                var dbUser = Environment.GetEnvironmentVariable("MYSQL_USER");
                var dbPassword = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (dbHost != null)
                {
                    connectionString = $"Server={dbHost};Port=3306;Database={dbName};Uid={dbUser};Pwd={dbPassword};";
                }

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<UserIdentity, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<UserIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

                var key = configuration!.GetSection("Jwt:Key").Value!;
                var issuer = configuration.GetSection("Jwt:Issuer").Value!;
                var audience = configuration.GetSection("Jwt:Audience").Value!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }

        private static IServiceCollection AddQueue(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var serviceBusConnectionString = configuration!.GetConnectionString("ServiceBusConnection") ?? Environment.GetEnvironmentVariable("ServiceBusConnection");

            services.AddSingleton(serviceProvider =>
            {
                return new ServiceBusClient(serviceBusConnectionString);
            });

            services.AddScoped<MailQueue>();

            return services;
        }
    }
}


