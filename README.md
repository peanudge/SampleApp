# Dev Plan

1. Item, Genre, Artist Simple API Server

- (DB, persistance layer) use ~~SQLite or in-memory~~ SQL Server in Docker as fast prototype.
- (BE, test) Add unit case about domain logic.
- [ ] (BE, domain layer) use hexagon-architecture for simple dependency graph.

2. Add logic in client app using RTK(Redux-ToolKit)

- (test) add unit test using jest

3. Think simple publishing. (MUI or Manual CSS)

- (test) add storybook

4. Deploy using Azure App Service.

# This project have three-layer

- API, as Web Layer receives requests and routes them to a service in the Domain or business layer
- Domain, as Domain Layer
- Infrastructure, as Persistence Layer to query for or modify the current state of our domain entities.

# DB Connection

Run following docker image and Update `ConnectionStrings` in `<root path>/projects/API/appsettings.Development.json`

- Create SQL Server docker

```bash
docker run --name SampleDB -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

if you use apple M1 or M2 chip, use this docker image `mcr.microsoft.com/azure-sql-edge` instead of `mcr.microsoft.com/mssql/server:2019-latest`.

```bash
docker run --name SampleDB -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge
```

- Make `store` database in `SampleDB` docker

```sql
IF NOT EXISTS (
    SELECT [name] FROM sys.databases WHERE [name] = N'store'
) CREATE DATABASE store
GO
```

- Add it in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "SQLServer": "Server=localhost,1434;Database=store;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;"
  }
}
```
