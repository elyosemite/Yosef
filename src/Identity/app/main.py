from fastapi import FastAPI
from .routers import token, users
from .config import settings

app = FastAPI(title="Identity Service")

app.include_router(token.router)
app.include_router(users.router)

@app.get("/")
async def root():
    return {"service": "identity-service", "realm": settings.keycloak_realm}
