# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy and restore project dependencies
COPY ["TestIvyInit.csproj", "./"]
RUN dotnet restore "TestIvyInit.csproj"
# Copy source and build
COPY . .
RUN dotnet build "TestIvyInit.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore
# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TestIvyInit.csproj" -c $BUILD_CONFIGURATION -o /app/publish
# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Set environment variables
ENV PORT=80
ENV ASPNETCORE_URLS=http://+:80
# Run app
ENTRYPOINT ["dotnet", "TestIvyInit.dll"]