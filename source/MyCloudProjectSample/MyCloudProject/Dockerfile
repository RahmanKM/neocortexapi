# Use the official Microsoft .NET 5 runtime image as the base for the runtime stage
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

# Install system dependencies required for System.Drawing.Common
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6-dev \
        libgdiplus \
        libx11-dev \
    && rm -rf /var/lib/apt/lists/*

# Use the official Microsoft .NET 5 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["MyCloudProject/MyCloudProject.csproj", "MyCloudProject/"]
COPY ["MyCloudProject.Common/MyCloudProject.Common.csproj", "MyCloudProject.Common/"]
COPY ["MyExperiment/MyExperiment.csproj", "MyExperiment/"]

RUN dotnet restore "MyCloudProject/MyCloudProject.csproj"

# Copy everything else and build the app
COPY . .
WORKDIR "/src/MyCloudProject"
RUN dotnet build "MyCloudProject.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MyCloudProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image that runs the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyCloudProject.dll"]
