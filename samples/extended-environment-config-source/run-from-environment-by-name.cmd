SET ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=%~dp0/Code/_adapters/EnvironmentVariables
SET ANYWHERE_ADAPTER_0_NAME=EnvironmentVariables
SET SECRET_VALUE=This secret value is obtained from the ENVIRONMENT VARIABLES

dotnet run --project Code/Code.csproj

PAUSE