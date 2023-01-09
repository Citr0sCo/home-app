FROM nginx:1.17.1-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY /dist/home-box-landing /usr/share/nginx/html

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["home-box-landing.api/home-box-landing.api.csproj", "home-box-landing.api/"]
RUN dotnet restore "home-box-landing.api/home-box-landing.api.csproj"
COPY . .
WORKDIR "/src/home-box-landing.api"
RUN dotnet build "home-box-landing.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "home-box-landing.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "home-box-landing.api.dll"]
