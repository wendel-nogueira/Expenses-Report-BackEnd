FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ExpensesReport.Users.API/ExpensesReport.Users.API.csproj", "ExpensesReport.Users.API/"]
COPY ["src/ExpensesReport.Users.Application/ExpensesReport.Users.Application.csproj", "ExpensesReport.Users.Application/"]
COPY ["src/ExpensesReport.Users.Core/ExpensesReport.Users.Core.csproj", "ExpensesReport.Users.Core/"]
COPY ["src/ExpensesReport.Users.Infrastructure/ExpensesReport.Users.Infrastructure.csproj", "ExpensesReport.Users.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Users.API/ExpensesReport.Users.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Users.API"
RUN dotnet build "ExpensesReport.Users.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Users.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Users.API.dll"]