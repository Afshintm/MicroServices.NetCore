docker pull mcr.microsoft.com/mssql/server:2017-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=M@nager1352" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest
REM these two command added but not tested.
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=M@nager1352" -p 1433:1433 -v c:/data:/host_data --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=M@nager1352" -p 1433:1433 -v c:/data:/var/opt/mssql/data/ --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest