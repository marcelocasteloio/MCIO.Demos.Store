FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Pricing/WebApi/MCIO.Demos.Store.Pricing.WebApi.csproj
RUN dotnet build Contexts/Pricing/WebApi/MCIO.Demos.Store.Pricing.WebApi.csproj -c Release --no-restore
RUN dotnet publish Contexts/Pricing/WebApi/MCIO.Demos.Store.Pricing.WebApi.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Pricing.WebApi.dll"]