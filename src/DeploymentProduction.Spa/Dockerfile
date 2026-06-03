# syntax=docker/dockerfile:1

FROM node:22-alpine AS build
WORKDIR /app

COPY src/DeploymentProduction.Spa/package*.json ./
RUN npm ci

COPY src/DeploymentProduction.Spa/ ./
RUN npm run build

FROM nginx:1.27-alpine AS final

RUN addgroup -S appgroup \
    && adduser -S appuser -G appgroup

COPY src/DeploymentProduction.Spa/nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

RUN chown -R appuser:appgroup /usr/share/nginx/html \
    && chown -R appuser:appgroup /var/cache/nginx \
    && chown -R appuser:appgroup /var/log/nginx \
    && chown -R appuser:appgroup /etc/nginx/conf.d \
    && touch /var/run/nginx.pid \
    && chown appuser:appgroup /var/run/nginx.pid

USER appuser

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]