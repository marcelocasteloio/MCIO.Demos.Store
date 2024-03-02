FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Ports/ClientWebBFF/MCIO.Demos.Store.Ports.ClientWebBFF.csproj
RUN dotnet build Ports/ClientWebBFF/MCIO.Demos.Store.Ports.ClientWebBFF.csproj -c Release --no-restore
RUN dotnet publish Ports/ClientWebBFF/MCIO.Demos.Store.Ports.ClientWebBFF.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Ports.ClientWebBFF.dll"]