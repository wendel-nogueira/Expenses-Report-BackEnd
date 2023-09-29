FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN ls -la
COPY ["src/ExpensesReport.Mail.Infrastructure/ExpensesReport.Mail.Infrastructure.csproj", "src/ExpensesReport.Mail.Infrastructure/"]
COPY ["src/ExpensesReport.Mail.Application/ExpensesReport.Mail.Application.csproj", "src/ExpensesReport.Mail.Application/"]
COPY ["src/ExpensesReport.Mail.Core/ExpensesReport.Mail.Core.csproj", "src/ExpensesReport.Mail.Core/"]
RUN dotnet restore "src/ExpensesReport.Mail.Infrastructure/ExpensesReport.Mail.Infrastructure.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Mail.Infrastructure"
RUN dotnet build "ExpensesReport.Mail.Infrastructure.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Mail.Infrastructure.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
