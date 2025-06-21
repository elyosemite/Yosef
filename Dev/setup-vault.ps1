# Configura o cliente Vault
$env:VAULT_ADDR = "http://localhost:8200"
$env:VAULT_TOKEN = "root-token"

# Habilita o AppRole
vault auth enable approle

# Cria uma policy básica
vault policy write myapp-policy - <<EOF
path "secret/data/myapp/*" {
  capabilities = ["read"]
}
EOF

# Cria uma role AppRole
vault write auth/approle/role/myapp-role policies=myapp-policy

# Obtém o Role ID e Secret ID
$roleId = (vault read -format=json auth/approle/role/myapp-role/role-id | ConvertFrom-Json).data.role_id
$secretId = (vault write -f -format=json auth/approle/role/myapp-role/secret-id | ConvertFrom-Json).data.secret_id

# Armazena em um arquivo temporário (não versionado)
@{
    RoleId = $roleId
    SecretId = $secretId
} | ConvertTo-Json | Out-File -FilePath "vault-creds.json" -Encoding utf8

# Cria um segredo de exemplo
vault kv put secret/myapp/database username="admin" password="s3cr3t"

Write-Host "Vault configurado! Role ID e Secret ID salvos em vault-creds.json."