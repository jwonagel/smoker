# # Stage 0, "build-stage", based on Node.js, to build and compile the frontend
# FROM arm32v7/node:latest as build-stage
# WORKDIR /app
# COPY package*.json /app/
# RUN npm install
# COPY ./ /app/

# ARG configuration=production

# #RUN npm run build -- --output-path=./dist/out --configuration $configuration 
# RUN node --max-old-space-size=2048 node_modules/@angular/cli/bin/ng build --prod --output-path=./dist/out

# Stage 1, based on Nginx, to have only the compiled app, ready for production with Nginx
FROM nginx:1.19
# COPY --from=build-stage /app/dist/out/ /usr/share/nginx/html

# # Copy the default nginx.conf provided by tiangolo/node-frontend

#COPY --from=build-stage /nginx.conf /etc/nginx/conf.d/default.conf
# Copy nginx Conf
COPY ./dist/out/ /usr/share/nginx/html 
COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
# COPY ./cert/roadchiefs.ddns.net/ /etc/ssl