#!/usr/bin/env pwsh

param(
    [switch]$all,
    [switch]$sqlite
)

$currentDir = Get-Location

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
    Write-Host "Gettting the User Secrets"
    $result = dotnet user-secrets list --json --project $"$currentDir/..src/ProjectManagement/ProjectManagement.Presentation" | Where-Object { $_ -notmatch "^//" } | ConvertFrom-Json
    Write-Host "$result"
    return $result
}

Foreach ($item in ,@(
    @($sqlite, "SQLite", "ProjectManagement.Presentation", "sqlite", 0)
)) {
    if(!$item[0]) {
        continue
    }

    Write-Host "===> $($item)"
    Write-Host "===> $($item[0])"
    Write-Host "===> $($item[1])"
    Write-Host "===> $($item[2])"
    Write-Host "===> $($item[3])"
    Write-Host "===> $($item[4])"

    Set-Location $"$currentDir/..src/ProjectManagement/$($item[2])/"
    if ($sqlite) {
        Write-Host "Starting $($item[1]) Migrations"
        Write-Host "$(Get-UserSecrets)"
        $connectionString = $(Get-UserSecrets)."globalSettings:$($item[3]):connectionString"
        dotnet ef database update --connection "$connectionString"
    } else {
        Write-Host "Connection string for a $($item[1]) database not found in secrets.json!"
    }
}

Set-Location "$currentDir"