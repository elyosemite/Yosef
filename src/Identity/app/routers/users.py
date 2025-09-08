from fastapi import APIRouter, HTTPException
from ..clients.keycloak_admin_client import KeycloakAdminClient

router = APIRouter(prefix="/identity", tags=["users"])
kc_admin = KeycloakAdminClient()

@router.post("/realms")
async def create_realm(name: str):
    return await kc_admin.create_realm(name)


@router.post("/users")
async def create_user(username: str, email: str, password: str, realm: str = None):
    return await kc_admin.create_user(username, email, password, realm)


@router.get("/users/{user_id}")
async def get_user(user_id: str, realm: str = None):
    try:
        return await kc_admin.get_user(user_id, realm)
    except Exception as e:
        raise HTTPException(status_code=404, detail=str(e))


@router.patch("/users/{user_id}")
async def update_user(user_id: str, updates: dict, realm: str = None):
    return await kc_admin.update_user(user_id, updates, realm)


@router.delete("/users/{user_id}")
async def delete_user(user_id: str, realm: str = None):
    return await kc_admin.delete_user(user_id, realm)


@router.post("/users/{user_id}/roles")
async def assign_role(user_id: str, role_name: str, realm: str = None):
    return await kc_admin.assign_role(user_id, role_name, realm)
