FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY ["src/ProjectManagement/", "src/ProjectManagement/"]
COPY ["dev/", "./dev/"]

RUN dotnet publish "src/ProjectManagement/ProjectManagement.Presentation/ProjectManagement.Presentation.csproj" -c Debug -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/dev ./dev
COPY --from=build /app/publish ./

# RUN apt-get update \
#     && apt-get install -y --no-install-recommends unzip \
#     && apt-get install -y --no-install-recommends curl \
#     && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg

ENTRYPOINT ["dotnet", "ProjectManagement.Presentation.dll"]
