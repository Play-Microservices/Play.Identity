FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Creates a non-root user with an explicit UID and add permission to access the /app folder
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ["src/Play.Identity.API/Play.Identity.API.csproj", "src/Play.Identity.API/"]
COPY ["src/Play.Identity.Contracts/Play.Identity.Contracts.csproj", "src/Play.Identity.Contracts/"]
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER --mount=type=secret,id=GH_PAT,dst=/GH_PAT --mount=type=secret,id=GH_USERNAME,dst=/GH_USERNAME \
  dotnet nuget add source --username "$(cat /GH_USERNAME)" --password "$(cat /GH_PAT)" --store-password-in-clear-text --name github "https://nuget.pkg.github.com/$(cat /GH_OWNER)/index.json"
RUN dotnet restore "src/Play.Identity.API/Play.Identity.API.csproj"
COPY ./src ./src
WORKDIR "/src/Play.Identity.API"
RUN dotnet publish "./Play.Identity.API.csproj" -c $BUILD_CONFIGURATION --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Play.Identity.API.dll"]
