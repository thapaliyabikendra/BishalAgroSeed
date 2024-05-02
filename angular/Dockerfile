# Stage 1: Build frontend
FROM node:16-alpine as node
RUN mkdir /app
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build:prod

# Stage 2: Configure Nginx
FROM nginx:1.17.1-alpine

# Configure ssl certificate
# RUN apk add --no-cache libressl
# RUN wget -O mkcert https://github.com/FiloSottile/mkcert/releases/download/v1.4.3/mkcert-v1.4.3-linux-amd64 && \
#     chmod +x mkcert && \
#     mv mkcert /usr/local/bin
# RUN mkdir -p /etc/nginx/certs/
# RUN mkcert -install && \
#     mkcert -cert-file /etc/nginx/certs/cert.pem -key-file /etc/nginx/certs/key.pem example.com localhost 127.0.0.1 ::1

COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=node /app/dist/BishalAgroSeed /usr/share/nginx/html
EXPOSE 80
# EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]
