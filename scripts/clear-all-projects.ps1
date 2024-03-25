# Obter o diretório pai
cd ..
$currentDirectory = Get-Location

# Verificar se o diretório atual existe
if (Test-Path $currentDirectory) {
    # Obter todas as pastas dentro do diretório atual e seus subdiretórios
    $folders = Get-ChildItem -Path $currentDirectory -Recurse -Directory | Where-Object { $_.Name -eq "bin" -or $_.Name -eq "obj" }

    # Remover cada pasta encontrada
    foreach ($folder in $folders) {
        Write-Host "Removendo pasta $($folder.FullName)"
        Remove-Item -Path $folder.FullName -Recurse -Force
    }
} else {
    Write-Host "O diretório atual não existe."
}