FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Analytics/WebApi/MCIO.Demos.Store.Analytics.WebApi.csproj
RUN dotnet build Contexts/Analytics/WebApi/MCIO.Demos.Store.Analytics.WebApi.csproj -c Release --no-restore
RUN dotnet publish Contexts/Analytics/WebApi/MCIO.Demos.Store.Analytics.WebApi.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Analytics.WebApi.dll"]