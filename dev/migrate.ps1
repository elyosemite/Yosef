#!/usr/bin/env pwsh

param(
    [switch]$all,
    [switch]$sqlite,
    [switch]$postgresql
)

# Caminho do diretório do script
$scriptDir = if ($PSScriptRoot) {
    $PSScriptRoot
} else {
    Split-Path -Parent $MyInvocation.MyCommand.Path
}

# Caminhos principais
$currentDir = Resolve-Path (Join-Path -Path $scriptDir -ChildPath "..")
$infraDir = Join-Path -Path $currentDir -ChildPath "src/ProjectManagement/ProjectManagement.Infrastructure"
$presentationDir = Join-Path -Path $currentDir -ChildPath "src/ProjectManagement/ProjectManagement.Presentation"

# Configuração padrão
if (-not $all -and -not $sqlite -and -not $postgresql) {
    $postgresql = $true
}

# Garante que dotnet-ef esteja disponível
function Ensure-DotnetEf{
    dotnet ef --version *> $null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "🛠️ Entity Framework Core tools not found. Installing..."
        dotnet tool install -g dotnet-ef
        $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
    }
}

Ensure-DotnetEf

# Função de busca de secrets (para futuro uso com Vault)
function Get-UserSecrets {
    Write-Host "🔐 Fetching secrets from user-secrets"
    $secrets = dotnet user-secrets list --json --project $presentationDir | Where-Object { $_ -notmatch "^//" } | ConvertFrom-Json
    return $secrets
}

# Simulação de uso local da connection string
# Em produção, você substituiria essa lógica por chamada a HashiCorp Vault
function Get-ConnectionString([string]$provider) {
    switch ($provider.ToLower()) {
        "postgresql" {
            return "Host=localhost;Port=5432;Username=postgres;Password=example123;Database=banco_docker;Include Error Detail=true"
        }
        "sqlite" {
            return "Data Source=project.db"
        }
        default {
            throw "❌ Unsupported provider: $provider"
        }
    }
}

# Execução das migrações
function Run-Migration([string]$provider, [string]$dbName) {
    $connectionString = Get-ConnectionString $provider
    Write-Host "`n🚀 Starting migration for $dbName"
    Write-Host "🔗 Using connection string: $connectionString"

    Push-Location $infraDir

    dotnet ef database update `
        --project "$infraDir/ProjectManagement.Infrastructure.csproj" `
        --startup-project "$presentationDir/ProjectManagement.Presentation.csproj" `
        --context "BrokerageContext" `
        --connection "$connectionString" `
        --no-build

    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Migration for $dbName succeeded"
    } else {
        Write-Host "❌ Migration for $dbName failed" -ForegroundColor Red
        exit 1
    }

    Pop-Location
}

# Execução conforme switches
if ($sqlite -or $all) {
    Run-Migration -provider "sqlite" -dbName "SQLite"
}

if ($postgresql -or $all) {
    Run-Migration -provider "postgresql" -dbName "PostgreSQL"
}

Write-Host "`n🏁 All requested migrations completed."
