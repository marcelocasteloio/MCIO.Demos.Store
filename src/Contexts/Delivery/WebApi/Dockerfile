FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Delivery/WebApi/MCIO.Demos.Store.Delivery.WebApi.csproj
RUN dotnet build Contexts/Delivery/WebApi/MCIO.Demos.Store.Delivery.WebApi.csproj -c Release --no-restore
RUN dotnet publish Contexts/Delivery/WebApi/MCIO.Demos.Store.Delivery.WebApi.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Delivery.WebApi.dll"]