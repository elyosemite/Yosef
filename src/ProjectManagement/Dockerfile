FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .

WORKDIR /src/ProjectManagement.Presentation
RUN dotnet restore
RUN dotnet publish ProjectManagement.Presentation.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "ProjectManagement.Presentation.dll"]