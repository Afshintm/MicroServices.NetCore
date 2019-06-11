FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1402



FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MicroServices/Identity.Management.Service/Identity.Management.Api/Identity.Management.Api.csproj", "MicroServices/Identity.Management.Service/Identity.Management.Api/"]
COPY ["MicroServices/Essence.Communication.Service/Essence.Communication.DbContexts/Essence.Communication.DbContexts.csproj", "MicroServices/Essence.Communication.Service/Essence.Communication.DbContexts/"]
COPY ["MicroServices/Services.Utilities/Services.Utilities.DataAccess/Services.Utilities.DataAccess.csproj", "MicroServices/Services.Utilities/Services.Utilities.DataAccess/"]
COPY ["MicroServices/Services.Utilities/Services.Utilities.Common/Services.Utilities.Common.csproj", "MicroServices/Services.Utilities/Services.Utilities.Common/"]
COPY ["MicroServices/Essence.Communication.Service/Essence.Communication.Models/Essence.Communication.Models.csproj", "MicroServices/Essence.Communication.Service/Essence.Communication.Models/"]
COPY ["Messaging/BuildingBlocks/BuildingBlocks.EventBus/BuildingBlocks.EventBus.csproj", "Messaging/BuildingBlocks/BuildingBlocks.EventBus/"]
RUN dotnet restore "MicroServices/Identity.Management.Service/Identity.Management.Api/Identity.Management.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Identity.Management.Service/Identity.Management.Api"
RUN dotnet build "Identity.Management.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Identity.Management.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT="Test"
ENTRYPOINT ["dotnet", "Identity.Management.Api.dll"]