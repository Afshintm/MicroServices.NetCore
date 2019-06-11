FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["MicroServices/Identity.Management.Service/Clients/MvcClient/MvcClient.csproj", "MicroServices/Identity.Management.Service/Clients/MvcClient/"]
RUN dotnet restore "MicroServices/Identity.Management.Service/Clients/MvcClient/MvcClient.csproj"
COPY . .
WORKDIR "/src/MicroServices/Identity.Management.Service/Clients/MvcClient"
RUN dotnet build "MvcClient.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MvcClient.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MvcClient.dll"]