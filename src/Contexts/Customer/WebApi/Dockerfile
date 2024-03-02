FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Customer/WebApi/MCIO.Demos.Store.Customer.WebApi.csproj
RUN dotnet build Contexts/Customer/WebApi/MCIO.Demos.Store.Customer.WebApi.csproj -c Release --no-restore
RUN dotnet publish Contexts/Customer/WebApi/MCIO.Demos.Store.Customer.WebApi.csproj -c Release -o out --no-build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Customer.WebApi.dll"]