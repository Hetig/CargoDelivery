FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CargoDelivery.API/CargoDelivery.API.csproj", "CargoDelivery.API/"]
COPY ["CargoDelivery.Storage/CargoDelivery.Storage.csproj", "CargoDelivery.Storage/"]
COPY ["CargoDelivery.Domain/CargoDelivery.Domain.csproj", "CargoDelivery.Domain/"]
RUN dotnet restore "CargoDelivery.API/CargoDelivery.API.csproj"
COPY . .
WORKDIR "/src/CargoDelivery.API"
RUN dotnet build "./CargoDelivery.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CargoDelivery.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CargoDelivery.API.dll"]
