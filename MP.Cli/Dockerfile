# Build custom HandBrakeCLI with FDK support and wrap it in a deb
FROM debian:buster-slim AS hbbuild
ARG HANDBRAKE_VERSION=1.3.3
ARG HANDBRAKE_URL=https://github.com/HandBrake/HandBrake/releases/download/${HANDBRAKE_VERSION}/HandBrake-${HANDBRAKE_VERSION}-source.tar.bz2
RUN echo "deb http://deb.debian.org/debian buster-backports main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian buster main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian-security/ buster/updates main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian buster-updates main contrib non-free" >> /etc/apt/sources.list && \
	apt-get update && apt-get install -y checkinstall curl autoconf automake autopoint build-essential cmake git \
	libass-dev libbz2-dev libfontconfig1-dev libfreetype6-dev libfribidi-dev libharfbuzz-dev libjansson-dev \
	liblzma-dev libmp3lame-dev libnuma-dev libogg-dev libopus-dev libsamplerate-dev libspeex-dev libtheora-dev \
	libtool libtool-bin libturbojpeg0-dev libvorbis-dev libx264-dev libx265-dev libxml2-dev libvpx-dev m4 make meson nasm \
	ninja-build patch pkg-config python tar zlib1g-dev meson nasm libva-dev libdrm-dev libfdk-aac-dev
WORKDIR /hb
RUN mkdir HandBrake; curl -# -L ${HANDBRAKE_URL} | tar xj --strip 1 -C HandBrake;
WORKDIR /hb/HandBrake
RUN ./configure --enable-fdk-aac --enable-x265 --enable-numa --enable-vce --enable-qsv --disable-gtk --arch=x86_64
WORKDIR /hb/HandBrake/build
RUN make
RUN checkinstall -y --install=no
WORKDIR /app

# Build the base runtime box, adding external software deps (like the custom HandBrake)
FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
COPY --from=hbbuild /hb/HandBrake/build/*.deb /tmp
RUN echo "deb http://deb.debian.org/debian buster-backports main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian buster main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian-security/ buster/updates main contrib non-free" >> /etc/apt/sources.list && \
	echo "deb http://deb.debian.org/debian buster-updates main contrib non-free" >> /etc/apt/sources.list && \
	apt-get update && apt-get install -y ffmpeg libgdiplus fdkaac libjansson4 libnvidia-encode1 libass9 \
	libfontconfig1 libfreetype6 libtheora0 libturbojpeg0 libxml2 zlib1g libdvdnav4 libdvdread4 libvorbis0a \
	libvorbisenc2 libx264-155 libx265-165 && dpkg -i /tmp/*.deb && apt-get -f install && rm -rf /var/lib/apt/lists/*
COPY MP.Core/Handbrake/handbrake.json /tmp/handbrake.json
RUN mkdir -p /tmp/mpwork /zmedia/plex; /usr/local/bin/HandBrakeCLI --preset-import-file /tmp/handbrake.json
WORKDIR /app

# Build environment for .NET
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
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MP.Cli.dll"]