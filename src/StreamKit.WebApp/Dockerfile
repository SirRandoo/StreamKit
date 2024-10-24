FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["src/StreamKit.WebApp/StreamKit.WebApp.csproj", "src/StreamKit.WebApp/"]
COPY ["src/StreamKit.Common.Data/StreamKit.Common.Data.csproj", "src/StreamKit.Common.Data/"]
COPY ["src/StreamKit.Common.Data.Abstractions/StreamKit.Common.Data.Abstractions.csproj", "src/StreamKit.Common.Data.Abstractions/"]
COPY ["src/StreamKit.Common.Communication/StreamKit.Common.Communication.csproj", "src/StreamKit.Common.Communication/"]
RUN dotnet restore "src/StreamKit.WebApp/StreamKit.WebApp.csproj"
COPY . .
WORKDIR "/src/src/StreamKit.WebApp"
RUN dotnet build "StreamKit.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StreamKit.WebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StreamKit.WebApp.dll"]
