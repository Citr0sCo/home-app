FROM nginx:1.17.1-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY /dist/home-box-landing /usr/share/nginx/html

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /web-api/app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /web-api/src
COPY ["/api/home-box-landing/home-box-landing.api/home-box-landing.api.csproj", "home-box-landing.api/"]
RUN dotnet restore "home-box-landing.api/home-box-landing.api.csproj"
COPY . .
WORKDIR "/web-api/src/home-box-landing.api"
RUN dotnet build "home-box-landing.api.csproj" -c Release -o /web-api/app/build

FROM build AS publish
RUN dotnet publish "home-box-landing.api.csproj" -c Release -o /web-api/app/publish

FROM base AS final
WORKDIR /web-api/app
COPY --from=publish /web-api/app/publish .
ENTRYPOINT ["dotnet", "home-box-landing.api.dll"]
