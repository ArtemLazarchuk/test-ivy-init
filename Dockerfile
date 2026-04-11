# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore (lock file speeds up reproducible restores)
COPY ["TestIvyInit.csproj", "packages.lock.json", "./"]
RUN dotnet restore "TestIvyInit.csproj"

# Copy everything and build
COPY . .
RUN dotnet build "TestIvyInit.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TestIvyInit.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV PORT=80
ENV ASPNETCORE_URLS="http://+:80"

# Run the application
ENTRYPOINT ["dotnet", "./TestIvyInit.dll"]
