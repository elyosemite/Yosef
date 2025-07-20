#!/usr/bin/env pwsh

param(
    [switch]$all,
    [switch]$sqlite
)

$scriptDir = if ($PSScriptRoot) {
    $PSScriptRoot
} else {
    Split-Path -Parent $MyInvocation.MyCommand.Path
}

$currentDir = Join-Path -Path $scriptDir -ChildPath ".."
$ProjectManagementAPI = Join-Path -Path $currentDir -ChildPath "src/ProjectManagement/ProjectManagement.Presentation"

function Get-EntityFrameworkDatabase {
    return $sqlite
}

if (!$all -and !$(Get-EntityFrameworkDatabase)) {
    $sqlite = $true
}

if ($all -or $(Get-EntityFrameworkDatabase)) {
    dotnet ef *> $null
    if($LASTEXITCODE -ne 0) {
        Write-Host "Entity Framework Core tools were not found in the dotnet global tools. Attempting to install"
        dotnet tool install dotnet-ef -g
    }
}

function Get-UserSecrets {
    # The dotnet cli command sometimes adds //BEGIN and //END comments to the output, Where-Object removes comments
    # to ensure a valid json
    Write-Host "Getting the User Secrets"
    $result = dotnet user-secrets list --json --project $ProjectManagementAPI | Where-Object { $_ -notmatch "^//" } | ConvertFrom-Json
    return $result
}

Foreach ($item in ,@(
    @($sqlite, "SQLite", "ProjectManagement.Infrastructure", "sqlite", 0)
)) {
    if(!$item[0]) {
        continue
    }

    Set-Location "$currentDir/src/ProjectManagement/$($item[2])/"

    if ($sqlite) {
        Write-Host "Starting $($item[1]) Migrations"
        $connectionString = $(Get-UserSecrets)."globalSettings:$($item[3]):connectionString"
        Write-Host "Using connection string: $($connectionString) in Migration"
        dotnet ef database update -- --connection "$connectionString"
    } else {
        Write-Host "Connection string for a $($item[1]) database not found in secrets.json!"
    }
}

Set-Location "$currentDir"