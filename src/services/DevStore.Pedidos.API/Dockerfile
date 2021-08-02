#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./services/DevStore.Pedidos.API/DevStore.Pedidos.API.csproj", "services/DevStore.Pedidos.API/"]
COPY ["./building blocks/DevStore.Core/DevStore.Core.csproj", "building blocks/DevStore.WebAPI.Core/"]
COPY ["./building blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building blocks/DevStore.WebAPI.Core/"]
COPY ["./building blocks/DevStore.MessageBus/DevStore.MessageBus.csproj", "building blocks/DevStore.MessageBus/"]

RUN dotnet restore "services/DevStore.Pedidos.API/DevStore.Pedidos.API.csproj"
COPY . .
WORKDIR "services/DevStore.Pedidos.API"
RUN dotnet build "DevStore.Pedidos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.Pedidos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.Pedidos.API.dll"]
