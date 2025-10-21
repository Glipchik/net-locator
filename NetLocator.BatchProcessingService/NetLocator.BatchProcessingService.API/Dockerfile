FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NetLocator.BatchProcessingService.API/NetLocator.BatchProcessingService.API.csproj", "NetLocator.BatchProcessingService.API/"]
COPY ["NetLocator.BatchProcessingService.Business/NetLocator.BatchProcessingService.Business.csproj", "NetLocator.BatchProcessingService.Business/"]
COPY ["NetLocator.BatchProcessingService.Shared/NetLocator.BatchProcessingService.Shared.csproj", "NetLocator.BatchProcessingService.Shared/"]
RUN dotnet restore "NetLocator.BatchProcessingService.API/NetLocator.BatchProcessingService.API.csproj"
COPY . .
WORKDIR "/src/NetLocator.BatchProcessingService.API"
RUN dotnet build "NetLocator.BatchProcessingService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetLocator.BatchProcessingService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetLocator.BatchProcessingService.API.dll"]
