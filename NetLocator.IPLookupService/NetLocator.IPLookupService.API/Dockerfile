FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetLocator.IPLookupService.API/NetLocator.IPLookupService.API.csproj", "NetLocator.IPLookupService.API/"]
COPY ["NetLocator.IPLookupService.Business/NetLocator.IPLookupService.Business.csproj", "NetLocator.IPLookupService.Business/"]
COPY ["NetLocator.IPLookupService.IpStack/NetLocator.IPLookupService.IpStack.csproj", "NetLocator.IPLookupService.IpStack/"]
COPY ["NetLocator.IPLookupService.Shared/NetLocator.IPLookupService.Shared.csproj", "NetLocator.IPLookupService.Shared/"]
RUN dotnet restore "NetLocator.IPLookupService.API/NetLocator.IPLookupService.API.csproj"
COPY . .
WORKDIR "/src/NetLocator.IPLookupService.API"
RUN dotnet build "./NetLocator.IPLookupService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NetLocator.IPLookupService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetLocator.IPLookupService.API.dll"]
