FROM nginx:latest
COPY nginx.conf /etc/nginx/nginx.conf
COPY cloudflare_ips.conf /etc/nginx/cloudflare_ips.conf
COPY certs/nginx-selfsigned.crt /etc/nginx/nginx-selfsigned.crt
COPY certs/nginx-selfsigned.key /etc/nginx/nginx-selfsigned.key