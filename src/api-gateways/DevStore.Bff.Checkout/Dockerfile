#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["api-gateways/DevStore.Bff.Checkout/DevStore.Bff.Checkout.csproj", "api-gateways/DevStore.Bff.Checkout/"]
COPY ["building-blocks/DevStore.WebAPI.Core/DevStore.WebAPI.Core.csproj", "building-blocks/DevStore.WebAPI.Core/"]
COPY ["building-blocks/DevStore.Core/DevStore.Core.csproj", "building-blocks/DevStore.Core/"]
COPY ["building-blocks/DevStore.MessageBus/DevStore.MessageBus.csproj", "building-blocks/DevStore.MessageBus/"]
RUN dotnet restore "api-gateways/DevStore.Bff.Checkout/DevStore.Bff.Checkout.csproj"
COPY . .
WORKDIR "api-gateways/DevStore.Bff.Checkout"
RUN dotnet build "DevStore.Bff.Checkout.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevStore.Bff.Checkout.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevStore.Bff.Checkout.dll"]