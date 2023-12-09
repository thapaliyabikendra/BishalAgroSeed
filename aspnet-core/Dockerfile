#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BishalAgroSeed.HttpApi.Host/BishalAgroSeed.HttpApi.Host.csproj", "src/BishalAgroSeed.HttpApi.Host/"]
COPY ["src/BishalAgroSeed.Application/BishalAgroSeed.Application.csproj", "src/BishalAgroSeed.Application/"]
COPY ["src/BishalAgroSeed.Application.Contracts/BishalAgroSeed.Application.Contracts.csproj", "src/BishalAgroSeed.Application.Contracts/"]
COPY ["src/BishalAgroSeed.Domain.Shared/BishalAgroSeed.Domain.Shared.csproj", "src/BishalAgroSeed.Domain.Shared/"]
COPY ["src/BishalAgroSeed.Domain/BishalAgroSeed.Domain.csproj", "src/BishalAgroSeed.Domain/"]
COPY ["src/BishalAgroSeed.EntityFrameworkCore/BishalAgroSeed.EntityFrameworkCore.csproj", "src/BishalAgroSeed.EntityFrameworkCore/"]
COPY ["src/BishalAgroSeed.HttpApi/BishalAgroSeed.HttpApi.csproj", "src/BishalAgroSeed.HttpApi/"]
RUN dotnet restore "src/BishalAgroSeed.HttpApi.Host/BishalAgroSeed.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/BishalAgroSeed.HttpApi.Host"
#RUN mv appsettings.Production.json appsettings.json
RUN dotnet build "BishalAgroSeed.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BishalAgroSeed.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BishalAgroSeed.HttpApi.Host.dll"]
