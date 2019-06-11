Echo Better to use MicroServices/build-all-db
REM dotnet ef migrations add ApplicationIdentity  -c ApplicationDbContext  -o Data/Migrations/ApplicationIdentityDb

REM dotnet ef migrations script -c ApplicationDbContext -o Data/Migrations/ApplicationIdentityDb.sql

REM dotnet ef database  update -c ApplicationDbContext 


REM dotnet ef migrations add Grants -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb

REM dotnet ef migrations script -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb.sql

REM dotnet ef database update -c PersistedGrantDbContext


REM dotnet ef migrations add Config -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb

REM dotnet ef migrations script -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb.sql

REM dotnet ef database update -c ConfigurationDbContext

REM dotnet run /seed
