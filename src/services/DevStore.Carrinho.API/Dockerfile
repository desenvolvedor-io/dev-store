#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./services/DevStore.Carrinho.API/DevStore.Carrinho.API.csproj", "services/DevStore.Carrinho.API/"]
COPY ["./building blocks/DevStore.Core/DevStore.Core.csproj", "building blocks/DevStore.WebAPI.Core/"]
COPY ["./building blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building blocks/DevStore.WebAPI.Core/"]
COPY ["./building blocks/DevStore.MessageBus/DevStore.MessageBus.csproj", "building blocks/DevStore.MessageBus/"]

RUN dotnet restore "services/DevStore.Carrinho.API/DevStore.Carrinho.API.csproj"
COPY . .
WORKDIR "services/DevStore.Carrinho.API"
RUN dotnet build "DevStore.Carrinho.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.Carrinho.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.Carrinho.API.dll"]
