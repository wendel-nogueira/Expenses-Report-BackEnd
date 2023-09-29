FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Identity.API/ExpensesReport.Identity.API.csproj", "ExpensesReport.Identity.API/"]
COPY ["src/ExpensesReport.Identity.Application/ExpensesReport.Identity.Application.csproj", "ExpensesReport.Identity.Application/"]
COPY ["src/ExpensesReport.Identity.Core/ExpensesReport.Identity.Core.csproj", "ExpensesReport.Identity.Core/"]
COPY ["src/ExpensesReport.Identity.Infrastructure/ExpensesReport.Identity.Infrastructure.csproj", "ExpensesReport.Identity.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Identity.API/ExpensesReport.Identity.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Identity.API"
RUN dotnet build "ExpensesReport.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Identity.API.dll"]