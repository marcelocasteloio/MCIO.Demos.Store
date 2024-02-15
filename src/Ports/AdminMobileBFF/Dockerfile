FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Ports/AdminMobileBFF/MCIO.Demos.Store.Ports.AdminMobileBFF.csproj
RUN dotnet build Ports/AdminMobileBFF/MCIO.Demos.Store.Ports.AdminMobileBFF.csproj -c Release --no-restore
RUN dotnet publish Ports/AdminMobileBFF/MCIO.Demos.Store.Ports.AdminMobileBFF.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Ports.AdminMobileBFF.dll"]