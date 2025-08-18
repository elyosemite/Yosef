FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY EventProcessor.Worker.csproj ./EventProcessor/
RUN dotnet restore EventProcessor/EventProcessor.Worker.csproj

# Copiar o restante dos arquivos e buildar
COPY . ./EventProcessor/
WORKDIR /src/EventProcessor
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EventProcessor.Worker.dll"]