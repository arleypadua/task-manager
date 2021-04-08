# task-manager

Implementation of a task manager built using C# and .NET 5

## 📃 Overview

The project is organized following the Hexagonal architecture and split accross the following projects:

### 💼 TaskManager.Core

The core this task manager, containing all pieces and use cases that implements the business rules of the task manager and its behaviors.
This project is isolated and doesn't depend on any external libraries, except the Framework libraries.

All different behaviors were built built using concurrent data structures that will make sure the task manager is thread safe when changing its state.

### 🌎 TaskManager.Api

The only adapter available that plugs into the core, offering an interface to interact with the task manager implemented within the core.
Not much effort was spent here, since this was not the scope of the task manager.

The endpoints available are documented [here](https://task-manager-arley.azurewebsites.net/swagger).

### 🧪 TaskManager.Tests

Project containing tests asserting all use cases available in the Core:

- Add process
- List processes (with sorting)
- Kill/KillGroup/KillAll

## 🏃‍♂️ Running

There are many ways to run it and they are listed below

### 🚀 Production deployment

This repository uses GitHub Actions to deploy the Docker image for the API onto a free Azure Web App resource.

This will be the easiest way as the code is already running in a server.
Access it via: https://task-manager-arley.azurewebsites.net/swagger

_P.S.: When hitting the link above, give it a few seconds as Azure might need to warm-up the API Server_

### 🚢 Locally with Docker

1. Checkout the project

```bash
git checkout https://github.com/arleypadua/task-manager
```

2. In the repository folder, build the docker image

```bash
# build the image
docker build ./src --file ./src/TaskManager.Api/Dockerfile --tag task-manager-api

# run the image
docker run -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development task-manager-api:latest
```

3. Go to `http://localhost:5000/swagger` and run the endpoints available

### 💻 Locally with dotnet CLI

1. Make sure you have the dotnet CLI installed for .NET 5

2. Checkout the project

```bash
git checkout https://github.com/arleypadua/task-manager
```

3. In the repository folder, run the following

```bash
# build the project
dotnet build ./src

# run the api
dotnet run --project ./src/TaskManager.Api/TaskManager.Api.csproj
```

4. Go to `http://localhost:5000/swagger` and run the endpoints available
