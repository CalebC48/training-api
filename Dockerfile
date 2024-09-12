# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./CAP.API ./
RUN dotnet restore

# Copy everything else and build
RUN dotnet publish ./CAP.API.csproj -c Release -o out

# Build runtime image
# This builds a seperate image for the runtime, without the bloat of build time dependencies
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
# Validate static file directory
RUN mkdir -p /app/wwwroot
ENTRYPOINT ["dotnet", "CAP.API.dll"]