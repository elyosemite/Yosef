# KeyCloak configuration

# 1. obter token admin (realm master)
ADMIN_TOKEN=$(curl -s -X POST "http://localhost:8080/realms/master/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "username=admin" \
  -d "password=admin" \
  -d "grant_type=password" \
  -d "client_id=admin-cli" | jq -r .access_token)

# 2. criar realm (opcional)
curl -X POST "http://localhost:8080/admin/realms" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"id":"myrealm","realm":"myrealm","enabled":true}'

# 3. criar client confidencial (identity-client)
curl -X POST "http://localhost:8080/admin/realms/myrealm/clients" \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "clientId":"identity-client",
    "enabled":true,
    "publicClient":false,
    "protocol":"openid-connect",
    "directAccessGrantsEnabled":true,
    "serviceAccountsEnabled":true,
    "redirectUris":["*"]
  }'
# Depois pegue o client secret via GET /admin/realms/{realm}/clients?clientId=identity-client
