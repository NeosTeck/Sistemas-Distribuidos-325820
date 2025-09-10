# TODO ESTO ES PARA HACER LA IMAGEN 

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["DinoApi.csproj","./"]
RUN dotnet restore "DinoApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DinoApi.csproj" -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration=Realese
RUN dotnet publish "DinoApi.csproj" -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DinoApi.dll" ]

