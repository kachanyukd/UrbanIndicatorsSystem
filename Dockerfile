# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["UrbanIndicatorsSystem.csproj", "./"]
RUN dotnet restore "UrbanIndicatorsSystem.csproj"

# Copy source code
COPY . .

# Generate code before build
WORKDIR /src/CodeGeneration
RUN dotnet run --project CodeGenerator.csproj

# Build application
WORKDIR /src
RUN dotnet build "UrbanIndicatorsSystem.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "UrbanIndicatorsSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

# Create healthcheck
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "UrbanIndicatorsSystem.dll"]
