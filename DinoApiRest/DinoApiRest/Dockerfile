
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["DinoApiRest.csproj", "./"]   # <- Cambia al nombre de tu proyecto
RUN dotnet restore "DinoApiRest.csproj"
COPY . .
RUN dotnet build "DinoApiRest.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "DinoApiRest.csproj" -c $configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DinoApiRest.dll"]
