#!/bin/bash
set -e

echo "=== Checking if dotnet is installed ==="

if command -v dotnet >/dev/null 2>&1; then
    echo "dotnet is installed in this machine";
else
    echo "dotnet is not install in this machine";
    exit 1;
fi

ls ./ProjectManagement.Infrastructure

echo "Installing dotnet-ef..."
dotnet tool install --global dotnet-ef --version 9.0.8

export PATH="$PATH:/root/.dotnet/tools"

echo "Showing the dotnet-ef on screen"
dotnet-ef

echo "Running the migration..."
dotnet-ef database update \
    --project ./ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj \
    --startup-project ./ProjectManagement.Presentation/ProjectManagement.Presentation.csproj \
    --context OrganizationContext \
    -- "GlobalSettings:PostgreSql:ConnectionString=\"Host=postgres;Username=postgres;Password=example123;Database=banco_docker\""

echo "=== Migration completed successfully ==="
