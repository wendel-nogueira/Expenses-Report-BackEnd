using Azure.Messaging.ServiceBus;
using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Core.repositories;
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

                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));
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

        private static IServiceCollection AddQueue(this IServiceCollection services)
        {
            var serviceBusConnectionString = "Endpoint=sb://expensesrepotort.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=T1iYaDPoboFthwg188t/hy/5EVRn72r+8+ASbCAK9Gc=";

            //Environment.GetEnvironmentVariable("ServiceBusConnectionString");

            services.AddSingleton(serviceProvider =>
            {
                return new ServiceBusClient(serviceBusConnectionString);
            });

            services.AddScoped<MailQueue>();

            return services;
        }
    }
}


