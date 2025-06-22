function Remove-Directory {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DirectoryPath
    )
    if (Test-Path $DirectoryPath -PathType Container) {
        Remove-Item -Path $DirectoryPath -Recurse -Force
        Write-Host "Diretório '$DirectoryPath' deletado com sucesso."
    } else {
        Write-Host "Diretório '$DirectoryPath' não encontrado."
    }
}

function Remove-File {
    param(
        [Parameter(Mandatory=$true)]
        [string]$FilePath
    )
    if (Test-Path $FilePath -PathType Leaf) {
        Remove-Item -Path $FilePath -Force
        Write-Host "Arquivo '$FilePath' deletado com sucesso."
    } else {
        Write-Host "Arquivo '$FilePath' não encontrado."
    }
}

function Clean-Project {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DirectoryPath,
        [Parameter(Mandatory=$true)]
        [string]$FilePath
    )
    Remove-Directory -DirectoryPath $DirectoryPath
    Remove-File -FilePath $FilePath
}

# Exemplo de uso:
# . .\clean.ps1
# Clean-Project -DirectoryPath ".\Migrations" -