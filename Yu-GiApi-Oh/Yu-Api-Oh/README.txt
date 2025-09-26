
El proyecto levanta 2 contenedores

1. SOAP Service: escucha en el puerto `9091` y expone el WSDL.
2. PostgreSQL: base de datos con usuario y contraseña configurados.

Para levantar los contendores se necesitan estos comandos:


Podman build -t yu-api-oh:1 .
Podman network create yugioh-net
Creación de la db:
Podman run -d --name yugioh-db --network yugioh-net -e POSTGRES_DB=yugioh_db -e POSTGRES_USER=yugioh_user -e POSTGRES_PASSWORD=yugioh_pass -p 5432:5432 postgres:16
Creación del contenedor de la api
Podman run -d --name yu-api-oh --network yugioh-net -p 9091:8000 yu-api-oh:1
