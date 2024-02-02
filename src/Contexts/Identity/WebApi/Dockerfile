FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

COPY ./src ./
RUN dotnet restore Contexts/Identity/WebApi/MCIO.Demos.Store.Identity.WebApi.csproj
RUN dotnet publish Contexts/Identity/WebApi/MCIO.Demos.Store.Identity.WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "MCIO.Demos.Store.Identity.WebApi.dll"]