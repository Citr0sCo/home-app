FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /web-api/app
EXPOSE 82

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /web-api/src
COPY ["/api/home-box-landing/HomeBoxLanding.Api/HomeBoxLanding.Api.csproj", "HomeBoxLanding.Api/"]
RUN dotnet restore "HomeBoxLanding.Api/HomeBoxLanding.Api.csproj"
WORKDIR "/web-api/src/HomeBoxLanding.Api"
COPY . .
RUN dotnet build "HomeBoxLanding.Api.csproj" -c Release -o /web-api/app/build

FROM build AS publish
RUN dotnet publish "HomeBoxLanding.Api.csproj" -c Release -o /web-api/app/publish

FROM base AS final
WORKDIR /web-api/app
COPY --from=publish /web-api/app/publish .

COPY /dist/home-box-landing /web-api/app/wwwroot

CMD ["dotnet", "/web-api/app/HomeBoxLanding.Api.dll"]