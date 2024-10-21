# Stage 1: Build the Vue.js application
FROM node:lts-alpine as build-stage

WORKDIR /app

# Install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the app files
COPY . .

# Build the app
RUN npm run build-docker

# Stage 2: Serve the app with Nginx
FROM nginx:stable-alpine as production-stage

# Copy the built files to Nginx
COPY --from=build-stage /app/dist /usr/share/nginx/html

# Copy the environment variable injection script
COPY env.sh /docker-entrypoint.d/env.sh
RUN chmod +x /docker-entrypoint.d/env.sh

# Expose Nginx port
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
