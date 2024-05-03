#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/BishalAgroSeed.AuthServer/BishalAgroSeed.AuthServer.csproj", "src/BishalAgroSeed.AuthServer/"]
COPY ["src/BishalAgroSeed.EntityFrameworkCore/BishalAgroSeed.EntityFrameworkCore.csproj", "src/BishalAgroSeed.EntityFrameworkCore/"]
COPY ["src/BishalAgroSeed.Domain/BishalAgroSeed.Domain.csproj", "src/BishalAgroSeed.Domain/"]
COPY ["src/BishalAgroSeed.Domain.Shared/BishalAgroSeed.Domain.Shared.csproj", "src/BishalAgroSeed.Domain.Shared/"]
RUN dotnet restore "src/BishalAgroSeed.AuthServer/BishalAgroSeed.AuthServer.csproj"
COPY . .
WORKDIR "/src/src/BishalAgroSeed.AuthServer"
RUN dotnet build "BishalAgroSeed.AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BishalAgroSeed.AuthServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BishalAgroSeed.AuthServer.dll"]