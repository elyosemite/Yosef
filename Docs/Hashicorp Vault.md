### Passos

- configurar user secrets para teste localmente; 🆗
- configurar script powershell para usar os valores já atribuídos no secrets.json; 🆗
- ler os valores atribuídos no secrets.json com a aplicação rodando; 🆗
- configurar o Hashicorp Vault;


> Para que Hashicorp Vault recomenda que a aplicação acesse o Vault por meio de um AppRole,
método recomendado para aplicações.

### Configuração Segura de Autenticação
Evite ``hard-coded`` credentials! Use métodos de autenticação seguros e dinâmicos:

Passos:
- Habilite o AppRole no Vault: `vault auth enable approle`
- Crie uma Role e Associe uma Política: `vault write auth/approle/role/myapp-role policies=myapp-policy`
- Obtenha o RoleId e SecretId:
    ```bash
    vault read auth/approle/role/myapp-role/role-id
    vault write -f auth/approle/role/myapp-role/secret-id
    ```
- Armazene o RoleId e SecretId em Variáveis de Ambiente
    ```bash
    # Exemplo (Linux):
    export VAULT_ROLE_ID="xxx"
    export VAULT_SECRET_ID="yyy"
    ```

- Código .NET para Autenticação:
    ```cs
    using VaultSharp;
    using VaultSharp.V1.AuthMethods.AppRole;

    var roleId = Environment.GetEnvironmentVariable("VAULT_ROLE_ID");
    var secretId = Environment.GetEnvironmentVariable("VAULT_SECRET_ID");

    var authMethod = new AppRoleAuthMethodInfo(roleId, secretId);
    var vaultClientSettings = new VaultClientSettings("http://vault-server:8200", authMethod);
    var vaultClient = new VaultClient(vaultClientSettings);
    ```
    