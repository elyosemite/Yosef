param(
    [bool]$CleanFirst = $false,
    [bool]$RestartDbConfiguration = $false
)

if ($CleanFirst -or $RestartDbConfiguration) {
    Write-Host "Executando limpeza antes do build..."
    . "$PSScriptRoot\clean.ps1"
    # Ajuste os caminhos conforme necessário
    Clean-Project -DirectoryPath ".\Migrations" -FilePath ".\LocalDatabase.db"
}

if ($RestartDbConfiguration) {
    Write-Host "Recriando as migrações do Entity Framework..."
    # Certifique-se de estar no diretório correto do projeto para rodar o comando abaixo
    dotnet ef migrations add InitialCreate
    Write-Host "Migrações do Entity Framework recriadas com sucesso."
    dotnet ef database update
}

Write-Host "Executando dotnet clean..."
dotnet clean

Write-Host "Executando dotnet build..."
dotnet build

Write-Host "Iniciando dotnet watch run..."
dotnet watch run