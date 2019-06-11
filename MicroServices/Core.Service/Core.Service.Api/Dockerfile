FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY MicroServices/Core.Service/Core.Service.Api/Core.Service.Api.csproj MicroServices/Core.Service/Core.Service.Api/
COPY MicroServices/Essence.Communication.Service/Essence.Communication.BusinessServices/Essence.Communication.BusinessServices.csproj MicroServices/Essence.Communication.Service/Essence.Communication.BusinessServices/
COPY MicroServices/Services.Utils/Services.Utils.csproj MicroServices/Services.Utils/
COPY MicroServices/Essence.Communication.Service/Essence.Communication.Models/Essence.Communication.Models.csproj MicroServices/Essence.Communication.Service/Essence.Communication.Models/
COPY Messaging/BuildingBlocks/BuildingBlocks.EventBus/BuildingBlocks.EventBus.csproj Messaging/BuildingBlocks/BuildingBlocks.EventBus/
COPY Messaging/BuildingBlocks/BuildingBlocks.EventBus.MessageQueue/BuildingBlocks.EventBus.MessageQueue.csproj Messaging/BuildingBlocks/BuildingBlocks.EventBus.MessageQueue/
RUN dotnet restore MicroServices/Core.Service/Core.Service.Api/Core.Service.Api.csproj
COPY . .
WORKDIR /src/MicroServices/Core.Service/Core.Service.Api
RUN dotnet build Core.Service.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Core.Service.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Core.Service.Api.dll"]
