FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Projects.API/ExpensesReport.Projects.API.csproj", "ExpensesReport.Projects.API/"]
COPY ["src/ExpensesReport.Projects.Application/ExpensesReport.Projects.Application.csproj", "ExpensesReport.Projects.Application/"]
COPY ["src/ExpensesReport.Projects.Core/ExpensesReport.Projects.Core.csproj", "ExpensesReport.Projects.Core/"]
COPY ["src/ExpensesReport.Projects.Infrastructure/ExpensesReport.Projects.Infrastructure.csproj", "ExpensesReport.Projects.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Projects.API/ExpensesReport.Projects.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Projects.API"
RUN dotnet build "ExpensesReport.Projects.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Projects.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Projects.API.dll"]