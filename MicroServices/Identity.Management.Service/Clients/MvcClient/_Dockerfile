FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5002/tcp
Expose 443
ENV	ASPNETCORE_URLS http://*:5002


FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["Clients/MvcClient/MvcClient.csproj", "Clients/MvcClient/"]
RUN dotnet restore "Clients/MvcClient/MvcClient.csproj"
COPY . .
WORKDIR "/src/Clients/MvcClient"
RUN dotnet build "MvcClient.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MvcClient.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MvcClient.dll"]