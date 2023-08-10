# .NET Core 6.0 runtime image as the base image
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitListener.csproj", "./"]
RUN dotnet restore "RabbitListener.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "RabbitListener.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "RabbitListener.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitListener.dll"]
