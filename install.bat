@echo off

dotnet publish -c Debug -r linux-arm64 --self-contained

scp HAL\bin\Debug\net8.0\linux-arm64\publish\* root@192.168.3.6:debug

cls

rem Connect to the remote machine and run the application
ssh root@192.168.3.6 "dropbearkey -t ecdsa -f /etc/dropbear/dropbear_ecdsa_host_key && mkdir -p /root/.vs-debugger && chmod 755 /root/.vs-debugger"

pause
