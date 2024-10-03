For starting this project you need to have installed .Net and PostgreSQL. After which start the database and adjust the credentials in appsettings.json  
After which migrate the database using (in CLI) `dotnet ef migrations add NameSomehow` and `dotnet ef database update`  
To start the project, type (in CLI) `dotnet build` and `dotnet run` or just `dotnet watch`
