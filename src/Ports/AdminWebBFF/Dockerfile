FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Ports/AdminWebBFF/MCIO.Demos.Store.Ports.AdminWebBFF.csproj
RUN dotnet build Ports/AdminWebBFF/MCIO.Demos.Store.Ports.AdminWebBFF.csproj -c Release --no-restore
RUN dotnet publish Ports/AdminWebBFF/MCIO.Demos.Store.Ports.AdminWebBFF.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Ports.AdminWebBFF.dll"]