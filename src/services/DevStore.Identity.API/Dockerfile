#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/DevStore.Identity.API/DevStore.Identity.API.csproj", "services/DevStore.Identity.API/"]
COPY ["building-blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building-blocks/DevStore.WebAPI.Core/"]
COPY ["building-blocks/DevStore.Core/DevStore.Core.csproj", "building-blocks/DevStore.Core/"]
COPY ["building-blocks/DevStore.MessageBus/DevStore.MessageBus.csproj", "building-blocks/DevStore.MessageBus/"]
RUN dotnet restore "services/DevStore.Identity.API/DevStore.Identity.API.csproj"
COPY . .
WORKDIR "services/DevStore.Identity.API"
RUN dotnet build "DevStore.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.Identity.API.dll"]