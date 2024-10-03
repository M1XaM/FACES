For starting this project you need to have installed .Net and PostgreSQL. After which start the database and adjust the credentials in FACES.Source/appsettings.json  
After which migrate the database using (in CLI) `dotnet ef migrations add NameSomehow` and `dotnet ef database update`  
To start the project, type (in CLI) `FACES.Source/dotnet build` and `FACES.Source/dotnet run` or just `FACES.Source/dotnet watch`  
For running unit tests build the solution file `dotnet build solution.sln` and `dotnet test`  
