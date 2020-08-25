FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app/WebApplication2
EXPOSE 5000

# Copy csproj and restore as distinct layers
COPY WebApplication2/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY WebApplication2/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/WebApplication2/out .
ENTRYPOINT ["dotnet", "WebApplication2.dll"]
