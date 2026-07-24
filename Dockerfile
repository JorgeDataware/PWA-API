# syntax=docker/dockerfile:1

# Build the application with the SDK, then run it from the smaller ASP.NET image.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PWA-API.csproj", "./"]
RUN dotnet restore "PWA-API.csproj"

COPY . .
RUN dotnet publish "PWA-API.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

# Configuration is supplied by the deployment environment.  Do not bake the
# repository's appsettings files (which may contain development secrets) into
# the resulting image.
RUN rm -f /app/publish/appsettings*.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# The deployment platform should terminate TLS and route its public URL here.
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

COPY --from=build /app/publish .

# The official .NET 10 runtime image provides this unprivileged user.
USER $APP_UID
# Render supplies PORT at runtime. Docker users who do not provide it keep 8080.
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} exec dotnet PWA-API.dll"]
