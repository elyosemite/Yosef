from pydantic import Field
from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    keycloak_server_url: str = Field(..., env="KEYCLOAK_SERVER_URL")
    keycloak_realm: str = Field(..., env="KEYCLOAK_REALM")
    identity_client_id: str = Field(..., env="IDENTITY_CLIENT_ID")
    identity_client_secret: str = Field(None, env="IDENTITY_CLIENT_SECRET")
    log_level: str = Field("info", env="LOG_LEVEL")

    class Config:
        env_file = ".env"
        env_file_encoding = "utf-8"

settings = Settings()
