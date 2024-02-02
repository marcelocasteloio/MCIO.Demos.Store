FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Ports/ClientMobileBFF/MCIO.Demos.Store.Ports.ClientMobileBFF.csproj
RUN dotnet publish Ports/ClientMobileBFF/MCIO.Demos.Store.Ports.ClientMobileBFF.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Ports.ClientMobileBFF.dll"]