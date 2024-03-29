FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app/ApiDemo
EXPOSE 5000

# Copy csproj and restore as distinct layers
COPY ApiDemo/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ApiDemo/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/ApiDemo/out .
ENTRYPOINT ["dotnet", "ApiDemo.dll"]
