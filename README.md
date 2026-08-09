# Play Microservices

This repository contains the **Play.Identity** service and the **Common** library used to share contracts between the Identity service and other services.

## Play.Identity

Service to control users and provide Identity Service functionality.

### Building the app

```bash
dotnet build
```

### Running the app

```bash
dotnet run
```

### Local development SSL certificate

Trust the local development HTTPS certificate:

```bash
dotnet dev-certs https --trust
```

### Running MongoDB locally

Run MongoDB with a persistent localhost volume:

```bash
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo
```

### Add reference to the Common library

If using the locally exported NuGet package:

```bash
dotnet add package Play.Common
```

### Credentials

Development credentials can be checked using:

```bash
dotnet user-secrets list
```

**Admin**

* Email: `admin@play.com`
* Password: `Pass@word1`

### Build the docker image

```powershell
$env:GH_OWNER="Play-Microservices"
$env:GH_USERNAME="[USERNAME HERE]"
$env:GH_PAT="[PAT HERE]"
$version="1.0.2"
docker build --secret id=GH_USERNAME --secret id=GH_OWNER --secret id=GH_PAT -t play.identity:$version .
```

```bash
export GH_OWNER="Play-Microservices"
export GH_USERNAME="[USERNAME HERE]"
export GH_PAT="[PAT HERE]"
version="1.0.2"

docker build \
  --secret id=GH_OWNER \
  --secret id=GH_USERNAME \
  --secret id=GH_PAT \
  -t play.identity:$version .
```


### Run the docker image

```powershell
$adminPass="[PASSWORD HERE]"
version="1.0.2"
docker run -it -=rm -p 5002:8080 --name identity -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq -e IdentitySettings__AdminUserPassword=$adminPass --network playinfrastructure_default play.identity:$version
```

```bash
adminPass="[PASSWORD HERE]"
version="1.0.2"
docker run -it --rm \
  -p 5002:8080 \
  --name identity \
  -e MongoDbSettings__Host=mongo \
  -e RabbitMQSettings__Host=rabbitmq \
  -e IdentitySettings__AdminUserPassword="$adminPass" \
  --network playinfrastructure_default \
  "play.identity:$version"
```

---

## Contract library

Library containing the published contracts shared between the Identity service and other services.

### Building the library

```bash
dotnet build
```

### Configure the local NuGet package source

This only needs to be done once. Replace `<Absolute_path_to_package_folder>` with the absolute path to the local package folder:

```bash
dotnet nuget add source "<Absolute_path_to_package_folder>" -n PlayEconomy
```

### Pack the library

To create the NuGet package and export it to the `packages` folder:

```bash
dotnet pack -o ../../../packages/
```

To specify a package version:

```bash
dotnet pack -o ../../../packages/ -p:PackageVersion=1.0.2
```

### Publish the package to GitHub Packages

Set the package version, GitHub owner, and GitHub Personal Access Token:

```powershell
$version="1.0.2"
$owner="Play-Microservices"
$gh_pat="[PAT HERE]"
```

Pack the library:

```powershell
dotnet pack src/Play.Identity.Contracts/ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=http://github.com/$owner/play.identity -o ../packages
```

Push the package to GitHub Packages:

```powershell
dotnet nuget push ../packages/Play.Identity.Contracts.$version.nupkg --api-key $gh_pat --source "github"
```
