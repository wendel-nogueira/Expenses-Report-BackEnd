FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Expenses.API/ExpensesReport.Expenses.API.csproj", "ExpensesReport.Expenses.API/"]
COPY ["src/ExpensesReport.Expenses.Application/ExpensesReport.Expenses.Application.csproj", "ExpensesReport.Expenses.Application/"]
COPY ["src/ExpensesReport.Expenses.Core/ExpensesReport.Expenses.Core.csproj", "ExpensesReport.Expenses.Core/"]
COPY ["src/ExpensesReport.Expenses.Infrastructure/ExpensesReport.Expenses.Infrastructure.csproj", "ExpensesReport.Expenses.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Expenses.API/ExpensesReport.Expenses.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Expenses.API"
RUN dotnet build "ExpensesReport.Expenses.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Expenses.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Expenses.API.dll"]