# Play.Identity
Service to control users and provide Identity Service functionality

## Building app
dotnet build

## Running app
dotnet run

## Running MongoDB with localhost volume
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

## Add reference to exported Common library
dotnet add package Play.Common