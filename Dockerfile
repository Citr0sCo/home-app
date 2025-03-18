# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /web-api/app
EXPOSE 82

# Stage 2: Build Angular Application
FROM node:20 AS angular-build
WORKDIR /web-ui/src

# Copy Angular project files
COPY ["/web/home-box-landing/", "./"]

# Install dependencies and build Angular application
RUN npm install
RUN npm run build --prod

# Stage 3: Build .NET Application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /web-api/src

# Copy only the project file to improve build caching
COPY ["/api/home-box-landing/HomeBoxLanding.Api/HomeBoxLanding.Api.csproj", "api/"]

# Restore dependencies
RUN dotnet restore "api/HomeBoxLanding.Api.csproj"

# Copy the entire source folder
WORKDIR "/web-api/src/api"
COPY . .

# Clean up any old build artifacts to ensure a fresh build
RUN rm -rf /web-api/src/api/**/obj /web-api/src/api/**/bin

# Build the application in Release mode
RUN dotnet build "HomeBoxLanding.Api.csproj" -c Release -o /web-api/app/build

# Stage 4: Publish .NET Application
FROM build AS publish
RUN dotnet publish "HomeBoxLanding.Api.csproj" -c Release -o /web-api/app/publish

# Stage 5: Final Image
FROM base AS final
WORKDIR /web-api/app

# Copy the published output
COPY --from=publish /web-api/app/publish .

# Copy Angular build output to wwwroot
COPY --from=angular-build /web-ui/src/dist/home-box-landing /web-api/app/wwwroot

# Set the entry point
CMD ["dotnet", "HomeBoxLanding.Api.dll"]
