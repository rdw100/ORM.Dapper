# ORM.Dapper
The ORM Dapper demonstration project was created using Visual Studio 2019 16.8.4, .NET 5, & Dapper 2.0.78. The objective is to demonstrate Dapper in a .NET 5 application using Unit of Work and Repository Patterns consuming Northwind data through Web API tested with Swagger.

## Setup
Setup instructions to run this project are as follows:
1. Open Project
   Download ORM.Dapper project and open it in Visual Studio 2019.

2. Create Database
   This project consumes a Northwind database.  A Northwind database was created in the default local database server instance, mssqllocaldb.   To create this database in your chosen environment, follow run-and-execute Northwind script instructions from the [NorthWind-Pubs Repo](https://github.com/Microsoft/sql-server-samples/tree/master/samples/databases/northwind-pubs).  

3. Verify Connection 
   A connection string is necessary to communicate with the database created above.  This project places the connection in the appsettings.json file.  Verify the connection in Visual Studio and make sure that it is pointing to your server.  For example, this project uses the following local database connection: 

     ```   Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true```

## Demo
The animated gif below demonstrates application execution.
![Blazor Chart GIF](https://github.com/rdw100/ORM.Dapper/blob/master/ORM.Dapper.WebApi/img/jC5L5GEd3y.gif?raw=true)