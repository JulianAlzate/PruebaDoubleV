# PruebaDoubleV

# --instalar docker
https://www.docker.com/products/docker-desktop/

# una vez instalado el docker, tomar el archivo nombrado init.sql, y establecerlo en una rota ejemplo de mi caso:C:\docker_init\init.sql
# en caso de que esta ruta cambie. debe de ser remplazada en la perte inferior donde esta la creacion de bd de docker, ya que este contiene funciones y demas

# crear el redis en el CMD ejecutar el comando:
docker run -d --name redis-server -p 6379:6379 redis

# crea bd en Docker powershell:
docker run -d `
  --name postgres-server `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=123456 `
  -e POSTGRES_DB=PruebaDoubleV `
  -p 5432:5432 `
  -v "C:\docker_init\init.sql:/docker-entrypoint-initdb.d/init.sql" `
  -v "C:\docker_volumes\postgres\data:/var/lib/postgresql/data" `
  postgres:16
  
# verificar que esta corriendo:
# docker ps

# esperar un momento, para que se cree todo.

# ver registros:
docker exec -it postgres-server psql -U postgres -d PruebaDoubleV

# verificar creación tabla:
\dt

# verificar creación funciones:
\df

# en caso de que luego de ejecutar el  \df se congele, escribir q y dar enter para hacer las demás consultas


# para ejecucion de consultar y validacion de creacion y demas, ejecutar para consultar registros:
SELECT * FROM ObtenerTickets(NULL, 1, 10);

# en caso de querer filtrar por uno enespecifico:
SELECT * FROM ObtenerTickets('julian', 1, 10);

# se esta forma se pueden ejecutar las demas funciones


# tener en cuenta los puertos, en caso de que cambien, debe de cambiarse en el setting y en el test, y poner el creado en el docker
