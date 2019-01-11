SET ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=%~dp0/Code/_adapters/KeyPerFile
SET ANYWHERE_ADAPTER_0_NAME=KeyPerFile
SET ANYWHERE_ADAPTER_0_DIRECTORY_PATH=%~dp0/Code/_configs/config

dotnet run --project Code/Code.csproj

PAUSE