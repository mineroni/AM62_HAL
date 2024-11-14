@echo off

set runPName=HAL

dotnet publish -c Debug -r linux-arm64 --self-contained

RemoteDeploy.exe

cls

rem Connect to the remote machine and run the application
ssh root@192.168.3.6 "./debug/%runPName%"

pause
