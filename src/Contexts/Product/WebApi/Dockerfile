FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Product/WebApi/MCIO.Demos.Store.Product.WebApi.csproj
RUN dotnet publish Contexts/Product/WebApi/MCIO.Demos.Store.Product.WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Product.WebApi.dll"]