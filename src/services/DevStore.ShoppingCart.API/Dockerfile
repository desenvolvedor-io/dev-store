#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/DevStore.ShoppingCart.API/DevStore.ShoppingCart.API.csproj", "services/DevStore.ShoppingCart.API/"]
COPY ["building-blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building-blocks/DevStore.WebAPI.Core/"]
COPY ["building-blocks/DevStore.Core/DevStore.Core.csproj", "building-blocks/DevStore.Core/"]
COPY ["building-blocks/DevStore.MessageBus/DevStore.MessageBus.csproj", "building-blocks/DevStore.MessageBus/"]
RUN dotnet restore "services/DevStore.ShoppingCart.API/DevStore.ShoppingCart.API.csproj"
COPY . .
WORKDIR "services/DevStore.ShoppingCart.API"
RUN dotnet build "DevStore.ShoppingCart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.ShoppingCart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.ShoppingCart.API.dll"]