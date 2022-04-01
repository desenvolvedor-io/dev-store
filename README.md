![image](https://user-images.githubusercontent.com/5068797/161198565-ac18c5ac-c0d9-4669-9568-b2009e944d77.png)

DevStore - A microservices e-commerce built with ASP.NET
=====================
A real-world reference application powered by [desenvolvedor.io](https://desenvolvedor.io/) implementing the most common and used technologies to share with the technical community the best way to develop full and complex applications with .NET

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/6518989bea914b348c92385dda05f93d)](https://www.codacy.com/manual/EduardoPires/DevStoreProject?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=EduardoPires/DevStoreProject&amp;utm_campaign=Badge_Grade)
[![Build status](https://ci.appveyor.com/api/projects/status/rl2ja69994rt3ei6?svg=true)](https://ci.appveyor.com/project/EduardoPires/DevStoreproject)

###### This project was inspired by [EShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers), however we want build this project by "our way" 

###### The EShopOnContainers is an awesome project however the code has some little "bad smells" and is more complex than the DevStore. We like to think of the DevStore as a simplified (but no less complex) version and written with more care in code and small details.

## Give a Star! :star:
If you liked the project or if DevStore is helping you, please give us a star ;)

![image](https://user-images.githubusercontent.com/5068797/161200961-af22100f-ef9f-43c4-8a56-7ee53dccb0be.png)

## Want to learn everything to build an app like this?  :mortar_board:
Check this online courses at [desenvolvedor.io](https://desenvolvedor.io) (only in portuguese)

- [ASP.NET Core Expert](https://desenvolvedor.io/formacao/asp-net-core-expert)
- [Software Architect](https://desenvolvedor.io/formacao/arquiteto-de-software)

## Architecture Overview

### The entire application is based in a unique solution with 7 API's and one web application (MVC)
![image](https://user-images.githubusercontent.com/5068797/161202409-edcf2f38-0714-4de5-927d-1a02be4501ec.png)

This is a reference application, each microservice has its own database and represents a bounded context (DDD concept).
There is a BFF / API Gateway to manage the Basket / Order / Payment requests and data structure from responses.

![image](https://user-images.githubusercontent.com/5068797/161207732-e4f67ce4-624d-4067-a756-67ee1bb553de.png)



## How to use:
- You will need the latest Visual Studio 2019 and the latest .NET Core SDK.
- ***Please check if you have installed the same runtime version (SDK) described in global.json***
- The latest SDK and tools can be downloaded from https://dot.net/core.

Also you can run the DevStore in Visual Studio Code (Windows, Linux or MacOS).

To know more about how to setup your enviroment visit the [Microsoft .NET Download Guide](https://www.microsoft.com/net/download)

## Technologies implemented:

- ASP.NET 5.0 (with .NET Core 5.0)
 - ASP.NET MVC Core 
 - ASP.NET WebApi Core with JWT Bearer Authentication
 - ASP.NET Identity Core
- Entity Framework Core 5.0
- .NET Core Native DI
- AutoMapper
- FluentValidator
- MediatR
- Swagger UI with JWT support
- .NET DevPack
- .NET DevPack.Identity

## Architecture:

- Full architecture with responsibility separation concerns, SOLID and Clean Code
- Domain Driven Design (Layers and Domain Model Pattern)
- Domain Events
- Domain Notification
- Domain Validations
- CQRS (Imediate Consistency)
- Event Sourcing
- Unit of Work
- Repository

## Disclaimer:
- **NOT** intended to be a definitive solution
- Beware to use in production way
- Maybe you don't need a lot of implementations that is included, try avoid the **over engineering**

## About the next versions
Watch our [RoadMap](https://github.com/EduardoPires/DevStoreProject/wiki/RoadMap) to know the new changes

## Pull-Requests 
Make a contact! Don't submit PRs for extra features, all the new features are planned

## Why DevStore?
The DevStore is an astronomical event in which the plane of Earth's equator passes through the center of the Sun, which occurs twice each year, around 20 March and 23 September. [Wikipedia](https://en.wikipedia.org/wiki/DevStore)

DevStore is also a series of publications (subtitle: "The Review of Scientific Illuminism") in book form that serves as the official organ of the A∴A∴, a magical order founded by Aleister Crowley :) [Wikipedia](https://en.wikipedia.org/wiki/The_DevStore)

## We are Online:
See the project running on <a href="http://DevStoreproject.azurewebsites.net" target="_blank">Azure</a>

## About:
The DevStore was developed by [Eduardo Pires](http://eduardopires.net.br) under the [MIT license](LICENSE).
