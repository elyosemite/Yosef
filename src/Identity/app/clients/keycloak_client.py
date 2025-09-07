import httpx
from typing import Optional
from ..config import settings

class KeycloakClient:
    def __init__(self):
        base = settings.keycloak_server_url.rstrip("/")
        self.token_url = f"{base}/realms/{settings.keycloak_realm}/protocol/openid-connect/token"
        # default client id / secret used for user-password grant (the "identity client")
        self.default_client_id = settings.identity_client_id
        self.default_client_secret = settings.identity_client_secret

    async def token_with_password(self, username: str, password: str) -> dict:
        data = {
            "grant_type": "password",
            "username": username,
            "password": password,
            "client_id": self.default_client_id,
        }
        if self.default_client_secret:
            data["client_secret"] = self.default_client_secret

        async with httpx.AsyncClient() as client:
            resp = await client.post(self.token_url, data=data, timeout=10.0)
            # if OK, Keycloak retorns access_token, refresh_token, expires_in, token_type, ...
            if resp.status_code != 200:
                # propagar erro com detalhe
                raise httpx.HTTPStatusError(
                    f"Keycloak token error: {resp.status_code} {resp.text}", request=resp.request, response=resp
                )
            return resp.json()

    async def token_with_client_credentials(self, client_id: str, client_secret: str) -> dict:
        data = {
            "grant_type": "client_credentials",
            "client_id": client_id,
            "client_secret": client_secret,
        }
        async with httpx.AsyncClient() as client:
            resp = await client.post(self.token_url, data=data, timeout=10.0)
            if resp.status_code != 200:
                raise httpx.HTTPStatusError(
                    f"Keycloak token error: {resp.status_code} {resp.text}", request=resp.request, response=resp
                )
            return resp.json()
