FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MicroServices/Inventory.Service/Inventory.Service.Api/Inventory.Service.Api.csproj", "MicroServices/Inventory.Service/Inventory.Service.Api/"]
RUN dotnet restore "MicroServices/Inventory.Service/Inventory.Service.Api/Inventory.Service.Api.csproj"
COPY . .
WORKDIR "/src/MicroServices/Inventory.Service/Inventory.Service.Api"
RUN dotnet build "Inventory.Service.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Inventory.Service.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Inventory.Service.Api.dll"]