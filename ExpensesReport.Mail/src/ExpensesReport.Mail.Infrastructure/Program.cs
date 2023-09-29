using Microsoft.Extensions.Hosting;
using ExpensesReport.Mail.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using ExpensesReport.Mail.Application.Configurations;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddOptions<SmtpConfiguration>()
        .Configure<IConfiguration>((smtpConfiguration, configuration) =>
        {
            var host = Environment.GetEnvironmentVariable("SMTP_HOST");
            var port = Environment.GetEnvironmentVariable("SMTP_PORT");
            var username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            var enableSsl = Environment.GetEnvironmentVariable("SMTP_ENABLESSL");

            smtpConfiguration.Host = host ?? configuration.GetValue<string>("SmtpConfiguration:Host");
            smtpConfiguration.Port = port != null ? int.Parse(port) : configuration.GetValue<int>("SmtpConfiguration:Port");
            smtpConfiguration.Username = username ?? configuration.GetValue<string>("SmtpConfiguration:Username");
            smtpConfiguration.Password = password ?? configuration.GetValue<string>("SmtpConfiguration:Password");
            smtpConfiguration.EnableSsl = enableSsl != null ? bool.Parse(enableSsl) : configuration.GetValue<bool>("SmtpConfiguration:EnableSsl");
        });

        services.AddSingleton<IMailServices, MailServices>();
    });

var host = builder.Build();

host.Run();
