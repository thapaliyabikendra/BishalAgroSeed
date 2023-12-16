# Stage 1: Build frontend
FROM node:latest as node
RUN mkdir /app
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build:prod

# Stage 2: Use mkcert to generate certificates and configure Nginx
FROM nginx:alpine

# Install mkcert dependencies
RUN apk add --no-cache libressl

# Install mkcert
RUN wget -O mkcert https://github.com/FiloSottile/mkcert/releases/download/v1.4.3/mkcert-v1.4.3-linux-amd64 && \
    chmod +x mkcert && \
    mv mkcert /usr/local/bin

# Create directory for certificates
RUN mkdir -p /etc/nginx/certs/

# Generate certificates using mkcert
RUN mkcert -install && \
    mkcert -cert-file /etc/nginx/certs/cert.pem -key-file /etc/nginx/certs/key.pem example.com localhost 127.0.0.1 ::1

# Copy nginx.conf with updated configurations
COPY nginx.conf /etc/nginx/nginx.conf

# Copy the built frontend files to Nginx HTML directory
COPY --from=node /app/dist/BishalAgroSeed /usr/share/nginx/html

# Expose port 443 for HTTPS
EXPOSE 80
EXPOSE 443

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
