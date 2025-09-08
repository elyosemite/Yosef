from pydantic import BaseModel

class UserTokenRequest(BaseModel):
    username: str
    password: str

class ServiceTokenRequest(BaseModel):
    client_id: str
    client_secret: str

class TokenResponse(BaseModel):
    access_token: str
    expires_in: int
    refresh_expires_in: int | None = None
    refresh_token: str | None = None
    token_type: str | None = None
    scope: str | None = None
