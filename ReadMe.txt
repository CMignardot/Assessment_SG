
Create a shared network
$ docker network create test_network

Run SQL server
$ docker container run -p 1433:1433 -d --name mssql -v mssql_data:/var/opt/mssql -e SA_PASSWORD=1StrongPassword! -e ACCEPT_EULA=Y --network=test_network mcr.microsoft.com/mssql/server:2019-latest

Run Ex1
$ docker container run -d -p 8080:80 --network=test_network --name myapp ex1

View the web page
Go to "localhost:8080"

NB:
The connection string used here for the DB is the following:
- Server=mssql;Database=Ex1;User=sa;Password=1StrongPassword!

The validations are not implemented to check correctness when adding or modifying a table
The delete cascades are not implemented to when removing a table