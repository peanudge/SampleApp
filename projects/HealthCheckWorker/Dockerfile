FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Step 1  - Building the project

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HealthCheckWorker.csproj", "./"]
RUN dotnet restore "./HealthCheckWorker.csproj"
COPY . .

WORKDIR "/src/."
RUN dotnet build "HealthCheckWorker.csproj" -c Release -o /app/build

# Step 2 - Publishing the project
FROM build AS publish
RUN dotnet publish "HealthCheckWorker.csproj" -c Release -o /app/publish

# Step 3 - Run the project
FROM base AS final 
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "HealthCheckWorker.dll" ]
