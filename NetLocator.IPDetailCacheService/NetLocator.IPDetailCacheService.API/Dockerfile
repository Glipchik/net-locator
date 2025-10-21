FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetLocator.IPDetailCacheService.API/NetLocator.IPDetailCacheService.API.csproj", "NetLocator.IPDetailCacheService.API/"]
COPY ["NetLocator.IPDetailCacheService.Business/NetLocator.IPDetailCacheService.Business.csproj", "NetLocator.IPDetailCacheService.Business/"]
COPY ["NetLocator.IPDetailCacheService.External/NetLocator.IPDetailCacheService.External.csproj", "NetLocator.IPDetailCacheService.External/"]
COPY ["NetLocator.IPDetailCacheService.Shared/NetLocator.IPDetailCacheService.Shared.csproj", "NetLocator.IPDetailCacheService.Shared/"]
RUN dotnet restore "NetLocator.IPDetailCacheService.API/NetLocator.IPDetailCacheService.API.csproj"
COPY . .
WORKDIR "/src/NetLocator.IPDetailCacheService.API"
RUN dotnet build "./NetLocator.IPDetailCacheService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NetLocator.IPDetailCacheService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetLocator.IPDetailCacheService.API.dll"]
