from fastapi import FastAPI
from .routers import token
from .config import settings

app = FastAPI(title="Identity Service (Keycloak facade)")

app.include_router(token.router)

@app.get("/")
async def root():
    return {"service": "identity-service", "realm": settings.keycloak_realm}
