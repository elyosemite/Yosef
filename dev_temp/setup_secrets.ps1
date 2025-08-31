#!/usr/bin/env pwsh

# You dont need to run this script. It will be replaced by Key Vault container in the future

param (
    [switch]$clear,
    [Parameter(ValueFromRemainingArguments = $true, Position=1)]
    $cmdArgs
)

$filePath = "dev/secrets.json"

if (!(Test-Path $filePath)) {
    Write-Warning "No secrets.json file found, please copy and modify the provided example";
    exit;
}

if ($clear -eq $true) {
    Write-Output "Deleting all existing user secrets"
}

$projects = @{
    ProjectManagement = "src/ProjectManagement/ProjectManagement.Presentation"
}

foreach ($key in $projects.keys) {
    if ($clear -eq $true) {
        dotnet user-secrets clear -p $projects[$key]
    }
    $output = Get-Content $filePath | & dotnet user-secrets set -p $projects[$key]
    Write-Output "$output - $key"
}
