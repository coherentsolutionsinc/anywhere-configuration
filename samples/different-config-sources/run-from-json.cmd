SET ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=%~dp0/Code/_adapters/Json
SET ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter
SET ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json
SET ANYWHERE_ADAPTER_0_PATH=%~dp0/Code/_configs/config.json

dotnet run --project Code/Code.csproj

PAUSE