#### run mssql with Docker

1. pull docker mssql image
    <br>`docker pull microsoft/mssql-server-linux`
    <br> `docker pull mysql:latest`                            
    `docker imagesdd` 
2. run the image
     <br>`docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD={pwd}' -p 1433:1433 -d microsoft/mssql-server-linux`
     <br> `docker run -itd --name mysql-test -p 3306:3306 -e MYSQL_ROOT_PASSWORD={pwd} mysql`
     <br>`docker ps`
3. test local db connection to local:1433 with login set above


#### run data migration with EF
1. install package EF core; {EF core.SqlServer, Pomelo EFcore.Mysql}
2. create Database/AppDbContext.cs: Model <- Bridger -> Db
3. inject service; configure conn string
4. add Db notation in Models for validation
5. install EF tool, and relevant
6. init migrations, update it { db|model|code first(context) }
  
      ```
      dotnet tool install --global dotnet-ef --version 3.1.4
      dotnet ef migrations add initialMigration --project Umi.API
      dotnet ef database update -p Umi.API
      ```

#### do Model <-> Dto with AutoMapper
1. Scan Profile to do Auto Map


#### do keyword search, data filter with API param, LINQ
1. From Query/Body/Form/Route/Service
2. usr FromQuery to do keyword search
3. LINQ(SQL statement)-> IQueryable(defer execution)-> Aggregate(ToList(), Count(), SingleOrDefault())
4. use parameter to group manage/make optional url params

#### do data verification with Data Annotation, ModelState
1. rule: DTO vs Model
2. ASP.NET Core: Data Annotation(rule), ModelState.IsValid(check), Error response(return)


#### refactor project with Async/Await
1. async await:  
```
valid return { void task task<T> IAsyncIEnumerable<T>}
await Expr: invoke - ...... - await only when need? 
naming function: DoxxxxxAsync
caller of async must be async
task<T>: return invoker; open new thread to continue+suspend current thread

Componet: 
1. aysnc functin: no need wait itself to finish, caller continue
2. call async: contain await 
```
2. multi-threading
3. multi-machine
4. kubernetes

5. step: repo -> controller to async


#### deployment with Docker
1. Registry
public Image, docker hub
2. Image
docker pull {image}
3. Container
run image:

image -> docker run -> container (local)
      -> docker pull -> docker run (remote) -> container
      
commit container:
container -> docker commit -> image -> docker push
docker login
 
```
docker run -e '{env}' -p '{port}' -d '{image}'
docker inspect bridge
```
build docker image
docker run -d -p 127.0.0.1:3000:80 44cb4ace5601
^^


#### Authorisation with JWT
1. 401(unauthorised) 403(forbidden)
2. Session: ? stateful login
3. JWT: stateless login; replace cookie; stored in Client; 
    good: distributed deployment with multiple server
    bad: unrevokable token; undecrypted msg-> using https SSL can avoid
4. alg: HEADER + PAYLOAD + SIG using RSA; server using private key to decrypt
5. SSO: pre-matured framework

#### Order
1. status: Pending -> (Processing -> (Completed -> Return | Declined)) | Cancelled
2. functions
    a. checkout POST api/shoppingCart/checkout
    b. view history GET api/orders
    c. get order GET api/orders/{id}
    d. pay POST api/orders/pay
