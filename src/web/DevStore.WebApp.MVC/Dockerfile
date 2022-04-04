#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["web/DevStore.WebApp.MVC/DevStore.WebApp.MVC.csproj", "web/DevStore.WebApp.MVC/"]
COPY ["building-blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building-blocks/DevStore.WebAPI.Core/"]
COPY ["building-blocks/DevStore.Core/DevStore.Core.csproj", "building-blocks/DevStore.Core/"]
RUN dotnet restore "web/DevStore.WebApp.MVC/DevStore.WebApp.MVC.csproj"
COPY . .
WORKDIR "web/DevStore.WebApp.MVC"
RUN dotnet build "DevStore.WebApp.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.WebApp.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.WebApp.MVC.dll"]