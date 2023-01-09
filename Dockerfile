FROM nginx:1.17.1-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY /dist/home-box-landing /usr/share/nginx/html

FROM php:7.4-cli
COPY /api /usr/share/nginx/html
WORKDIR /usr/share/nginx/html
CMD [ "php", "./web-api.php" ]