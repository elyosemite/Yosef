# app/routers/token.py
from fastapi import APIRouter, HTTPException, status
from ..clients.keycloak_client import KeycloakClient
from ..schemas import UserTokenRequest, ServiceTokenRequest
import httpx

router = APIRouter(prefix="/identity", tags=["identity"])

kc = KeycloakClient()

@router.post("/token/user", response_model=dict)
async def token_user(body: UserTokenRequest):
    """
    Troca username+password por tokens no Keycloak (grant_type=password).
    O client_id/secret vem do IDENTITY_CLIENT_ID / IDENTITY_CLIENT_SECRET (config).
    """
    try:
        token = await kc.token_with_password(body.username, body.password)
        # devolve a resposta do Keycloak (você pode filtrar ou adaptar)
        return token
    except httpx.HTTPStatusError as e:
        # decodifica erro (400/401)
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail=f"Keycloak error: {e.response.text}")
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@router.post("/token/service", response_model=dict)
async def token_service(body: ServiceTokenRequest):
    """
    Client credentials flow (service-to-service).
    Entrada: client_id + client_secret (fornecidos pelo serviço requisitante).
    """
    try:
        token = await kc.token_with_client_credentials(body.client_id, body.client_secret)
        return token
    except httpx.HTTPStatusError as e:
        # usualmente 400/401 se credenciais invalidas
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail=f"Keycloak error: {e.response.text}")
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
