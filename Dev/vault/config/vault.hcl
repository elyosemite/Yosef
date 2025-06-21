# vault/config/vault.hcl
storage "file" {
  path = "/vault/data"
}

# Configuração do listener corrigida
listener "tcp" {
  address     = "0.0.0.0:8200"
  tls_disable = 1
}

api_addr = "http://0.0.0.0:8200"
ui = true
