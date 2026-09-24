# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore separately to take advantage of Docker's layer cache.
COPY ["global.json", "./"]
COPY ["NumbeSalud.Front/NumbeSalud.Front.csproj", "NumbeSalud.Front/"]
RUN dotnet restore "NumbeSalud.Front/NumbeSalud.Front.csproj"

COPY . .
RUN dotnet publish "NumbeSalud.Front/NumbeSalud.Front.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway supplies PORT at runtime. Use 8080 locally if it is absent.
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080
ENTRYPOINT ["sh", "-c", "dotnet NumbeSalud.Front.dll --urls http://0.0.0.0:${PORT:-8080}"]
