#### use docker run mssql on mac/linux

1. pull docker mssql image
    <br>`docker pull microsoft/mssql-server-linux`
    <br> `docker pull mysql:latest`                            
    `docker imagesdd` 
2. run the image
     <br>`docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD={pwd}' -p 1433:1433 -d microsoft/mssql-server-linux`
     <br> `docker run -itd --name mysql-test -p 3306:3306 -e MYSQL_ROOT_PASSWORD={pwd} mysql`
     <br>`docker ps`
3. test local db connection to local:1433 with login set above


#### use EF dotnet sdk to do data migration and update
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

#### use AutoMapper to do Model -> Dto
1. Scan Profile to do Auto Map


#### api param, keyword, data filter
1. From Query/Body/Form/Route/Service
2. usr FromQuery to do keyword search
3. LINQ(SQL statement)-> IQueryable(defer execution)-> Aggregate(ToList(), Count(), SingleOrDefault())
4. use parameter to group manage/make optional url params, 


