<p align="center">
    <img alt="logo" src="https://user-images.githubusercontent.com/5068797/161198565-ac18c5ac-c0d9-4669-9568-b2009e944d77.png#gh-light-mode-only" />
    <img alt="logo" src="https://user-images.githubusercontent.com/5068797/161364257-0c1d81f6-62ac-4192-93f8-836b4ce0fd06.png#gh-dark-mode-only" />
</p>

# A microservices e-commerce reference application built with ASP.NET 9

A real-world reference application powered by [desenvolvedor.io](https://desenvolvedor.io/) <img alt="Brasil" src="https://user-images.githubusercontent.com/5068797/161345649-c7184fdc-2bc3-42a9-8fb6-6ffee9c8f9c2.png" width="20" height="14" /> implementing the most common and used technologies to share with the technical community the best way to develop full and complex applications with .NET

This project was inspired by [EShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers). However, the real motivation was to build it by "our way."

The EShopOnContainers is an awesome project. However, the code has some "bad smells." We found it difficult to start learning/using EShopOnContainers compared to DevStore. We like to think of the DevStore as a simplified (but no less complex) version and written with more care in code and small details. We also focused only on the Web architecture with ASP.NET.

## ⭐ Give a Star!

If you liked the project or if DevStore is helping you, please give us a star ;)

<p align="center">
    <img alt="DevStore" src="https://user-images.githubusercontent.com/5068797/164293734-a72fbeeb-0965-4413-a624-29e1c56c25df.png" />
</p>

## :mortar_board: Want to learn everything to build an app like this?

Check these online courses at [desenvolvedor.io](https://desenvolvedor.io) (only in Portuguese)

- [ASP.NET Core Expert](https://desenvolvedor.io/formacao/asp-net-core-expert)
- [Software Architect](https://desenvolvedor.io/formacao/arquiteto-de-software)

## 💻 Technologies / Components implemented

- .NET 9
    - ASP.NET MVC Core
    - ASP.NET WebApi
    - ASP.NET Minimal API
    - ASP.NET Identity Core
    - Refresh Token
    - JWT with rotating public/private keys
    - gRPC
    - Background Services
    - Entity Framework Core

- Components / Services
    - RabbitMQ
    - EasyNetQ
    - Refit
    - Polly
    - Bogus
    - Dapper
    - FluentValidator
    - MediatR
    - Swagger UI with JWT support
    - NetDevPack
    - NetDevPack.Identity
    - NetDevPack.Security.JWT

- Hosting
    - IIS
    - NGINX
    - Docker (with composing)

## 🗺️ Architecture:

### Complete architecture implementing the most important and used concerns as:

- Hexagonal Architecture
- Clean Code
- Clean Architecture
- DDD - Domain Driven Design (Layers and Domain Model Pattern)
- Domain Events
- Domain Notification
- Domain Validations
- CQRS (Immediate Consistency)
- Retry Pattern
- Circuit Breaker
- Unit of Work
- Repository
- Specification Pattern
- API Gateway / BFF

---

## 🔍 Architecture Overview

The entire application is based on a unique solution with 7 APIs and one web application (MVC)

<p align="center">
    <img alt="read before" src="https://user-images.githubusercontent.com/5068797/161202409-edcf2f38-0714-4de5-927d-1a02be4501ec.png" />
</p>

---

This is a reference application, each microservice has its own database and represents a bounded context (DDD concept).
There is a BFF / API Gateway to manage the Basket / Order / Payment requests and data structure from responses.

<p align="center">
    <img alt="read before" src="https://user-images.githubusercontent.com/5068797/161207732-e4f67ce4-624d-4067-a756-67ee1bb553de.png" />
</p>

---

## 🏁 Getting Started

You can run the DevStore project on any operating system. **Make sure you have installed docker in your environment.** ([Get Docker Installation](https://docs.docker.com/get-docker/))

Clone the DevStore's repository and navigate to the `/docker` folder and then:

### Run the DevStore application on Docker

```bash
docker compose up
```

### Build local images and run onDocker

This docker compose will provide one database container per each API service.

```bash
docker compose -f docker-compose-local.yml up --build
```

### Only run dependency resources:

This docker compose will provide necessary services for all API services.

```bash
docker compose -f docker-common-resources.yml up -d
```

---

### Run with Visual Studio / Visual Studio Code:

You will need:

- Docker
- MS-SQL Server instance (or container)
- RabbitMQ

So just run in the `/docker` folder the command:

```bash
docker compose -f docker-common-resources.yml up -d
```

### If you want Visual Studio with F5 and debug experience:

- You will need at least Visual Studio 2022 and .NET 9.
- The latest SDK and tools can be downloaded from https://dot.net/core
- Set up the solution to start multiple projects and hit F5

![image](https://user-images.githubusercontent.com/5068797/161358024-bd5754b6-61e3-47f2-bd17-bd3c32ec4bdd.png)

---

### If you want Visual Studio Code experience:

- Open the VSCode on the root directory (solution file)
- Create a new launch.json at VSCode debug section and use the configuration below to set up the option "Start all
  projects"

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "DevStore MVC WebApp",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/web/DevStore.WebApp.MVC/bin/Debug/net9.0/DevStore.WebApp.MVC.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/web/DevStore.WebApp.MVC",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore Billing API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.Billing.API/bin/Debug/net9.0/DevStore.Billing.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.Billing.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore Catalog API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.Catalog.API/bin/Debug/net9.0/DevStore.Catalog.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.Catalog.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore Customers API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.Customers.API/bin/Debug/net9.0/DevStore.Customers.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.Customers.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore Identity API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.Identity.API/bin/Debug/net9.0/DevStore.Identity.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.Identity.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore Orders API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.Orders.API/bin/Debug/net9.0/DevStore.Orders.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.Orders.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore ShoppingCart API",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/services/DevStore.ShoppingCart.API/bin/Debug/net9.0/DevStore.ShoppingCart.API.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/services/DevStore.ShoppingCart.API",
            "console": "internalConsole",
            "stopAtEntry": false
        },
        {
            "name": "DevStore BFF Checkout",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/api-gateways/DevStore.Bff.Checkout/bin/Debug/net9.0/DevStore.Bff.Checkout.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/api-gateways/DevStore.Bff.Checkout",
            "console": "internalConsole",
            "stopAtEntry": false
        }
    ],
    "compounds": [
        {
            "name": "Start all projects",
            "configurations": [
                "DevStore MVC WebApp",
                "DevStore Billing API",
                "DevStore Catalog API",
                "DevStore Customers API",
                "DevStore Identity API",
                "DevStore Orders API",
                "DevStore ShoppingCart API",
                "DevStore BFF Checkout"
            ],
            "stopAll": true
        }
    ]
}   
```

## ⚠️ Disclaimer

- This is not an architectural template or bootstrap model for new apps
- All implementations were made for the real world, but the goal is to share knowledge
- In case it is too many implementations included, remove the excess and try to avoid **over-engineering**

## 🙌 Contributing

Open an issue and let's discuss! Do not submit PRs for undiscussed or unapproved features.

If you want to help us, choose an approved issue and implement it.

## 🚀 We are online

See the project running on <a href="https://devstore.academy" target="_blank">DevStore official instance</a>

## 💭 About

DevStore was proudly developed by [desenvolvedor.io](https://desenvolvedor.io/)
❤<img alt="Brasil" src="https://user-images.githubusercontent.com/5068797/161345649-c7184fdc-2bc3-42a9-8fb6-6ffee9c8f9c2.png" width="20" height="14" />
team under the [MIT license](LICENSE).
