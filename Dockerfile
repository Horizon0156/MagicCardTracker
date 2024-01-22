FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim-amd64 AS build
WORKDIR /src
COPY Sources .
RUN dotnet publish MagicCardTracker.Pwa -c Release /p:UseAppHost=false -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /publish/wwwroot .
COPY Config/nginx.conf /etc/nginx/nginx.conf
