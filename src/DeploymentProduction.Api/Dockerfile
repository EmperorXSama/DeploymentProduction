# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY DeploymentProduction.slnx ./
COPY src/DeploymentProduction.Api/DeploymentProduction.Api.csproj src/DeploymentProduction.Api/

RUN dotnet restore src/DeploymentProduction.Api/DeploymentProduction.Api.csproj

FROM restore AS publish
WORKDIR /src

COPY src/DeploymentProduction.Api/ src/DeploymentProduction.Api/

RUN dotnet publish src/DeploymentProduction.Api/DeploymentProduction.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

USER $APP_UID

EXPOSE 8080

ENTRYPOINT ["dotnet", "DeploymentProduction.Api.dll"]