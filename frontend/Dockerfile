FROM node:18-alpine3.15 AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx AS final
### COPY NGINX CONFIG FROM LOCAL FOLDER (WE MUST CREATE IT BEFORE COPYING)
COPY nginx.conf /etc/nginx/nginx.conf 
COPY --from=build /usr/src/app/dist/frontend /usr/share/nginx/html
