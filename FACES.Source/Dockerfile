# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application and build it
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
WORKDIR /app/out 
COPY --from=build /app/out .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "FACES.Source.dll"]
