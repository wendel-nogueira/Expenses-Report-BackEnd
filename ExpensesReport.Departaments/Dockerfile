FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Departaments.API/ExpensesReport.Departaments.API.csproj", "ExpensesReport.Departaments.API/"]
COPY ["src/ExpensesReport.Departaments.Application/ExpensesReport.Departaments.Application.csproj", "ExpensesReport.Departaments.Application/"]
COPY ["src/ExpensesReport.Departaments.Core/ExpensesReport.Departaments.Core.csproj", "ExpensesReport.Departaments.Core/"]
COPY ["src/ExpensesReport.Departaments.Infrastructure/ExpensesReport.Departaments.Infrastructure.csproj", "ExpensesReport.Departaments.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Departaments.API/ExpensesReport.Departaments.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Departaments.API"
RUN dotnet build "ExpensesReport.Departaments.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Departaments.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Departaments.API.dll"]