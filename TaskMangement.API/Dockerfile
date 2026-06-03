# =========================
# BUILD STAGE
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# copy everything
COPY . .

# restore
RUN dotnet restore TaskMangement.API/TaskMangement.API.csproj

# build
WORKDIR /src/TaskMangement.API
RUN dotnet build TaskMangement.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# publish
RUN dotnet publish TaskMangement.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "TaskMangement.API.dll"]