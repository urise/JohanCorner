mkdir %AasDBScriptLogDir%
del %AasDBScriptLogDir%\*.log

for /r Scripts %%i in (*.sql) do sqlcmd -S %AasDBServerName% -d %AasDBDatabaseName% -U %AasDBUserName% -P %AasDBPassword% -o %AasDBScriptLogDir%\%%~ni.log -i %%i
