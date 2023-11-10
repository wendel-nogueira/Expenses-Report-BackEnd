using ExpensesReport.Projects.Core.Repositories;
using ExpensesReport.Projects.Infrastructure.Persistence.Context;
using ExpensesReport.Projects.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpensesReport.Projects.Infrastructure
{
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

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ProjectsDbContext>((serviceProvider, options) =>
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

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();

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
}
