import httpx
from typing import Optional
from ..config import settings

class KeycloakAdminClient:
    def __init__(self):
        base = settings.keycloak_server_url.rstrip("/")
        self.base_url = base
        self.realm = settings.keycloak_realm

        self.admin_username = "admin"
        self.admin_password = "admin"
        self.admin_realm = "master"
        self.admin_client_id = "admin-cli"

        self._token: Optional[str] = None

    async def _get_admin_token(self) -> str:
        token_url = f"{self.base_url}/realms/{self.admin_realm}/protocol/openid-connect/token"
        data = {
            "grant_type": "password",
            "client_id": self.admin_client_id,
            "username": self.admin_username,
            "password": self.admin_password,
        }

        headers = {"Content-Type": "application/x-www-form-urlencoded"}

        async with httpx.AsyncClient() as client:
            resp = await client.post(token_url["access_token"], data=data, headers=headers)
            if resp.status_code != 200:
                raise Exception(f"Error getting admin token: {resp.status_code} {resp.text}")
            token = resp.json()["access_token"]
            self._token = token
            return token

    async def _headers(self) -> dict:
        token = await self._get_admin_token()
        return {"Authorization": f"Bearer {token}", "Content-Type": "application/json"}

    async def create_realm(self, name: str):
        url = f"{self.base_url}/admin/realms"
        data = {"id": name, "realm": name, "enabled": True}
        async with httpx.AsyncClient() as client:
            resp = await client.post(url, headers=await self._headers(), json=data)
            if resp.status_code not in (201, 409):
                # 201 Created, 409 Already exists
                resp.raise_for_status()
            return {"realm": name}

    async def create_user(self, username: str, email: str, password: str, realm: Optional[str] = None):
        realm = realm or self.realm
        url = f"{self.base_url}/admin/realms/{realm}/users"
        data = {
            "username": username,
            "email": email,
            "enabled": True,
            "credentials": [{"type": "password", "value": password, "temporary": False}],
        }
        async with httpx.AsyncClient() as client:
            resp = await client.post(url, headers=await self._headers(), json=data)
            if resp.status_code not in (201, 409):
                resp.raise_for_status()
            return {"username": username, "realm": realm}

    async def get_user(self, user_id: str, realm: Optional[str] = None):
        realm = realm or self.realm
        url = f"{self.base_url}/admin/realms/{realm}/users/{user_id}"
        async with httpx.AsyncClient() as client:
            resp = await client.get(url, headers=await self._headers())
            if resp.status_code != 200:
                raise Exception(f"Error getting user: {resp.status_code} {resp.text}")
            return resp.json()

    async def update_user(self, user_id: str, updates: dict, realm: Optional[str] = None):
        realm = realm or self.realm
        url = f"{self.base_url}/admin/realms/{realm}/users/{user_id}"
        async with httpx.AsyncClient() as client:
            resp = await client.put(url, headers=await self._headers(), json=updates)
            if resp.status_code not in (204, 200):
                resp.raise_for_status()
            return {"updated": True}

    async def delete_user(self, user_id: str, realm: Optional[str] = None):
        realm = realm or self.realm
        url = f"{self.base_url}/admin/realms/{realm}/users/{user_id}"
        async with httpx.AsyncClient() as client:
            resp = await client.delete(url, headers=await self._headers())
            if resp.status_code not in (204, 404):
                resp.raise_for_status()
            return {"deleted": resp.status_code == 204}

    async def assign_role(self, user_id: str, role_name: str, realm: Optional[str] = None):
        realm = realm or self.realm
        role_url = f"{self.base_url}/admin/realms/{realm}/roles/{role_name}"
        async with httpx.AsyncClient() as client:
            role_resp = await client.get(role_url, headers=await self._headers())
            if role_resp.status_code != 200:
                raise Exception(f"Role not found: {role_name}")
            role = role_resp.json()

            user_role_url = f"{self.base_url}/admin/realms/{realm}/users/{user_id}/role-mappings/realm"
            assign_resp = await client.post(user_role_url, headers=await self._headers(), json=[role])
            if assign_resp.status_code not in (204, 201):
                assign_resp.raise_for_status()
            return {"assigned": True, "role": role_name}
