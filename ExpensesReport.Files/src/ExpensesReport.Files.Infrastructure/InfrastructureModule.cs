using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpensesReport.Files.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddAuthentication();

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
