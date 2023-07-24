FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /project 
 
COPY ["projects/Cart.API/Cart.API.csproj", "projects/Cart.API/"]
RUN dotnet restore "projects/Cart.API/Cart.API.csproj"
COPY . .

WORKDIR "/project/projects/Cart.API"
RUN dotnet build "Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart.API.dll"]

