# Dev Environment

- Docker SQL Sever

```curl
docker run --name ToySQLServer -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=MyPass@word' -e 'MSSQL_PID=Express' -p 1434:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Your password should follow the SQL Server default password policy, otherwise the container can't set up SQL Server and will stop working.


```bash
docker exec -it ToySQLServer "bash"
```

Connect docker using Bash, And Create DB user, Assign role

```bash
mssql@9d9f4475a793:/$ /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "MyPass@word"
1> CREATE LOGIN catalog_srv WITH PASSWORD = 'P@ssw0rd';
2> CREATE DATABASE Store;
3> GO
1> USE Store;
2> CREATE USER catalog_srv;
3> GO
Changed database context to 'Store'.
1> EXEC sp_addrolemember N'db_owner', N'catalog_srv';
2> GO
```