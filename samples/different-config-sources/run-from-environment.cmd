SET ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=%~dp0/Code/_adapters/EnvironmentVariables
SET ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.AnyWhereEnvironmentVariablesConfigurationSourceAdapter
SET ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables
SET ANYWHERE_ADAPTER_0_PREFIX=SECRET
SET SECRET_VALUE=This secret value is obtained from the ENVIRONMENT VARIABLES

dotnet run --project Code/Code.csproj

PAUSE