#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./web/Thelema.WebApp.MVC/Thelema.WebApp.MVC.csproj", "web/Thelema.WebApp.MVC/"]
COPY ["./building blocks/Thelema.Core/Thelema.Core.csproj", "building blocks/Thelema.WebAPI.Core/"]
COPY ["./building blocks/Thelema.WebAPI.Core/Thelema.WebAPI.Core.csproj", "building blocks/Thelema.WebAPI.Core/"]

RUN dotnet restore "web/Thelema.WebApp.MVC/Thelema.WebApp.MVC.csproj"
COPY . .
WORKDIR "web/Thelema.WebApp.MVC"
RUN dotnet build "Thelema.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Thelema.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Thelema.WebApp.MVC.dll"]
