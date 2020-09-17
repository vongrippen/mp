#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
RUN apt-get update && apt-get install -y ffmpeg libgdiplus && rm -rf /var/lib/apt/lists/*
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MP.Cli/MP.Cli.csproj", "MP.Cli/"]
COPY ["MP.Core/MP.Core.csproj", "MP.Core/"]
COPY ["MP.Core.Sqlite/MP.Core.Sqlite.csproj", "MP.Core.Sqlite/"]
COPY ["MP.Core.Npgsql/MP.Core.Npgsql.csproj", "MP.Core.Npgsql/"]
COPY ["MP.Core.SqlServer/MP.Core.SqlServer.csproj", "MP.Core.SqlServer/"]
RUN dotnet restore "MP.Cli/MP.Cli.csproj"
COPY . .
WORKDIR "/src/MP.Cli"
RUN dotnet build "MP.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MP.Cli.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN mkdir -p /zmedia/plex
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MP.Cli.dll"]