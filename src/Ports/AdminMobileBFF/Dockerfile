FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

COPY ./NuGet.Config ./
COPY ./nuget/ ./nuget/

WORKDIR /App

COPY ./src/Ports/AdminMobileBFF ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Ports.AdminMobileBFF.dll"]