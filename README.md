# PruebaDoubleV

# Instalar docker
https://www.docker.com/products/docker-desktop/

# Una vez instalado el docker, tomar el archivo nombrado init.sql, y establecerlo en una ruta ejemplo de mi caso: C:\docker_init\init.sql en caso de que esta ruta cambie. debe de ser remplazada en la parte inferior donde esta la creación de bd de docker, ya que este contiene funciones y demas

# Crear el redis en el CMD ejecutar el comando:
``` 
docker run -d --name redis-server -p 6379:6379 redis
``` 

# Crea bd en Docker powershell:
``` 
docker run -d `
  --name postgres-server `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=123456 `
  -e POSTGRES_DB=PruebaDoubleV `
  -p 5432:5432 `
  -v "C:\docker_init\init.sql:/docker-entrypoint-initdb.d/init.sql" `
  -v "C:\docker_volumes\postgres\data:/var/lib/postgresql/data" `
  postgres:16
  ``` 
# Verificar que esta corriendo:
``` 
docker ps
``` 

# Esperar un momento, para que se cree todo.

# Ver registros:
``` 
docker exec -it postgres-server psql -U postgres -d PruebaDoubleV
``` 

# Verificar creación tabla:
``` 

\dt
``` 


# Verificar creación funciones:
``` 
\df
``` 

# En caso de que luego de ejecutar el  \df se congele, escribir q y dar enter para hacer las demás consultas


# Para ejecución de consultar y validación de creación y demas, ejecutar para consultar registros:
``` 
SELECT * FROM ObtenerTickets(NULL, 1, 10);
```

# En caso de querer filtrar por uno en especifico:
``` 
SELECT * FROM ObtenerTickets('julian', 1, 10);
``` 

# De esta forma se pueden ejecutar las demas funciones, se llama por el nombre de la función y se le pasan los parametros, segun el init,tener en cuenta los puertos, en caso de que cambien, debe de cambiarse en el setting y en el test, y poner el creado en el docker
