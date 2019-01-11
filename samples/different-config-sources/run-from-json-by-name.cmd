SET ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=%~dp0/Code/_adapters/Json
SET ANYWHERE_ADAPTER_0_NAME=Json
SET ANYWHERE_ADAPTER_0_PATH=%~dp0/Code/_configs/config.json

dotnet run --project Code/Code.csproj

PAUSE