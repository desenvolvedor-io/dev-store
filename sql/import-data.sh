# Wait 90 seconds to starting
sleep 90s
# Generating database
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "MyDB@123" -i generate-database.sql