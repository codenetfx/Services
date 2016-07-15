PowerShell -Command "Set-ExecutionPolicy Unrestricted -Force"

IF "%ComputeEmulatorRunning%"=="false" PowerShell .\PSScripts\ServiceStartup.ps1 -EnvironmentName %AriaEnvironment% -ServerRole Services >> "%TEMP%\StartupLog.txt" 2>&1

EXIT /B %errorlevel%