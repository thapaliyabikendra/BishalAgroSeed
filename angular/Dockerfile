#stage 1
FROM node:latest as node
RUN mkdir /app
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build:prod

#stage 2
FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=node /app/dist/BishalAgroSeed /usr/share/nginx/html
EXPOSE 80
