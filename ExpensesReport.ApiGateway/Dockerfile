FROM nginx:latest

ENV USERS_MICROSERVICE_IP users_microservice_ip
ENV IDENTITIES_MICROSERVICE_IP identities_microservice_ip
ENV PROJECTS_MICROSERVICE_IP projects_microservice_ip
ENV DEPARTAMENTS_MICROSERVICE_IP departaments_microservice_ip

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]