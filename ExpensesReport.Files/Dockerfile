FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Files.API/ExpensesReport.Files.API.csproj", "ExpensesReport.Files.API/"]
COPY ["src/ExpensesReport.Files.Application/ExpensesReport.Files.Application.csproj", "ExpensesReport.Files.Application/"]
COPY ["src/ExpensesReport.Files.Infrastructure/ExpensesReport.Files.Infrastructure.csproj", "ExpensesReport.Files.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Files.API/ExpensesReport.Files.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Files.API"
RUN dotnet build "ExpensesReport.Files.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Files.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Files.API.dll"]