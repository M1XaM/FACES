For starting this project you need to have installed .Net and PostgreSQL. After which start the database and adjust the credentials in FACES.Source/appsettings.json  
After which migrate the database using (in CLI) `dotnet ef migrations add NameSomehow` and `dotnet ef database update`  
To start the project, type (in CLI) `FACES.Source/dotnet build` and `FACES.Source/dotnet run` or just `FACES.Source/dotnet watch`  

For running unit tests build the solution file `dotnet build solution.sln` and `dotnet test`  

For running the docker containers just type `docker-compose up --build` after which `docker-compose down` will stop them
Make sure that you have installed Docker and that PostgreSQL port is free to use (the default port is `5432`)  