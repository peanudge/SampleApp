# Projects using the ideal pattern

1. Item, Genre, Artist Simple API Server

- Unit Tests
- Slicing Layer Tests
  - Inmemory Database
- Integrated Tests (API Level using `WebApplicationFactory`)
  - Mock using `Moq`
- DTO Mapper manually
- Validation (using `FluentValidation`)
- ASP.NET Custom Filter

  - Resource Existing Filter (`ItemExistsAttribute.cs`)
  - Global Exception Hanlder (Global Filter)
  - (TODO) Performance Check Filter

- (TODO) HAXAGON Architecture
- HATEOS REST API https://en.wikipedia.org/wiki/HATEOAS

2. Add logic in client app using RTK(Redux-ToolKit)

- (test) add unit test using jest

3. Think simple publishing. (MUI or Manual CSS)

- (test) add storybook

4. Deploy using Azure App Service.

# How to make SQL DB Connection

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

# Docker environment

```bash

```
