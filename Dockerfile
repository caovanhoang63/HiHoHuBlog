FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node:20-alpine AS node-build
WORKDIR /src

COPY package.json package-lock.json ./

RUN npm ci

COPY . .

RUN npx tailwindcss -i wwwroot/app.css -o wwwroot/app.min.css

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["HiHoHuBlog.csproj", "./"]
RUN dotnet restore "HiHoHuBlog.csproj"

COPY . .
COPY --from=node-build /src/wwwroot/app.min.css wwwroot/
COPY --from=node-build /src/node_modules ./node_modules

WORKDIR "/src/"
RUN dotnet build "HiHoHuBlog.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HiHoHuBlog.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiHoHuBlog.dll"]
