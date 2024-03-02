FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Gateways/General/MCIO.Demos.Store.Gateways.General.csproj
RUN dotnet build Gateways/General/MCIO.Demos.Store.Gateways.General.csproj -c Release --no-restore
RUN dotnet publish Gateways/General/MCIO.Demos.Store.Gateways.General.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Gateways.General.dll"]