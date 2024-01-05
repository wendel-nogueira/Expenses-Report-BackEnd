FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

RUN apt update \
&& apt install -y musl-dev \
&& ln -s /usr/lib/x86_64-linux-musl/libc.so /lib/libc.musl-x86_64.so.1

RUN apt-get update && apt-get install -y fontconfig libc6 libc6-dev libgtk2.0-0 libnss3 libatk-bridge2.0-0 libx11-xcb1 libxcb-dri3-0 libdrm-common libgbm1 libasound2 libappindicator3-1 libxrender1 libfontconfig1 libxshmfence1

COPY ["src/ExpensesReport.Export.API/ExpensesReport.Export.API.csproj", "ExpensesReport.Export.API/"]
COPY ["src/ExpensesReport.Export.Application/ExpensesReport.Export.Application.csproj", "ExpensesReport.Export.Application/"]
COPY ["src/ExpensesReport.Export.Core/ExpensesReport.Export.Core.csproj", "ExpensesReport.Export.Core/"]
COPY ["src/ExpensesReport.Export.Infrastructure/ExpensesReport.Export.Infrastructure.csproj", "ExpensesReport.Export.Infrastructure/"]
RUN dotnet restore "ExpensesReport.Export.API/ExpensesReport.Export.API.csproj"
COPY . .
WORKDIR "/src/src/ExpensesReport.Export.API"
RUN dotnet build "ExpensesReport.Export.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpensesReport.Export.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /src/src/ExpensesReport.Export.API/Images ./Images
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpensesReport.Export.API.dll"]