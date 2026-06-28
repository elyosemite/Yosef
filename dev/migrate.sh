#!/bin/bash
set -e

echo "=== Checking if dotnet is installed ==="

if command -v dotnet >/dev/null 2>&1; then
    echo "dotnet is installed in this machine"
else
    echo "dotnet is not installed in this machine"
    exit 1
fi

echo "Listing the contents of the current directory:"
ls ./

echo "Listing the contents of the ProjectManagement.Infrastructure directory:"
ls ./ProjectManagement.Infrastructure

echo "Installing dotnet-ef..."
dotnet tool install --global dotnet-ef --version 9.0.8

export PATH="$PATH:/root/.dotnet/tools"

echo "Running the migration for the ProjectManagement..."
dotnet-ef database update \
    --project ./ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj \
    --startup-project ./ProjectManagement.Presentation/ProjectManagement.Presentation.csproj \
    --context BrokerageContext \
    --connection "Host=postgres;Port=5432;Username=postgres;Password=example123;Database=banco_docker;Include Error Detail=true"

echo "=== Migration completed successfully ==="
