FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8090

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DuelistApi.csproj","./"]
RUN dotnet restore "DuelistApi.csproj"
COPY . .
RUN dotnet build "DuelistApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuelistApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuelistApi.dll"]
