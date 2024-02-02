param (
    [string]$version
)

dotnet pack -c Release ..\MCIO.Demos.Store.BuildingBlock.Core.csproj /p:Version="$version" --include-symbols --include-source
dotnet nuget push ..\bin\Release\MCIO.Demos.Store.BuildingBlock.Core.$version.nupkg --source mcio-demos-store
dotnet nuget push ..\bin\Release\MCIO.Demos.Store.BuildingBlock.Core.$version.symbols.nupkg --source mcio-demos-store
