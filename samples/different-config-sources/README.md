# Different Configuration Sources

This sample demonstrates a minimalistic console application that consumes configuration from `.json` configuration file or from directory using `key-per-file` approach.

## What is inside?

**run-from-json.cmd**

The command file that configures environment variables to use _Json_ configuration adapter from `Code/_adapters/Json` with configuration from `Code/_configs/config.json`.

The successfull command execution results in the: 

> secret-value = This secret value is obtained from the JSON

**run-from-key-per-file.cmd**

The command file that configures environment variables to use _KeyPerFile_ configuration adapter from `Code/_adapters/KeyPerFile` with configuration from `Code/_configs/config` directory.

The successfull command execution results in the: 

> secret-value = This secret value is obtained from the KEY-PER-FILE

**run-from-environment.cmd**

The command file that configures environment variables to use _EnvironmentVariables_ configuration adapter from `Code/_adapters/EnvironmentVariables` with configuration from environment variables prefixed with _SECRET_.

The successfull command execution results in the: 

> _VALUE = This secret value is obtained from the ENVIRONMENT VARIABLES

The **by-name** command files have the same purpose as described above but use well known adapters shorthands.

**Code/Code.csproj**

The minimalistic project with only entry point configuration.

Files:
* `Program.cs` - an entry point where `HostBuilder` and `AnyWhereConfiguration` are configured.

## Key points

**Program.cs**

The **AnyWhereConfiguration** is implemented as an extension methods for `IConfigurationBuilder` interface. The configuration process is very simple and consists from the call to `AddAnyWhereConfiguration` method inside `ConfigureAppConfiguration` method.

``` csharp
new HostBuilder()
    .ConfigureAppConfiguration(
        config =>
        {
            config.AddAnyWhereConfiguration();
        })
    .UseConsoleLifetime()
    .Build()
    .Run();
```

## How to use?

**Visual Studio**

The following steps will configure the project to use _Json_ configuration adapter from `Code/_adapters/Json` with configuration from `Code/_configs/config.json`.

1. Open `/Code/Properties/launchSettings.json` and uncomment the following lines:

    ``` json
    "workingDirectory": "<path>/anywhere-configuration/samples",
    "environmentVariables": {
      "ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH": "different-config-sources/Code/_adapters/Json",
      "ANYWHERE_ADAPTER_0_TYPE_NAME": "CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter",
      "ANYWHERE_ADAPTER_0_ASSEMBLY_NAME": "CoherentSolutions.Extensions.Configuration.AnyWhere.Json",
      "ANYWHERE_ADAPTER_0_PATH": "different-config-sources/Code/_configs/config.json"
    }
    ```

2. Replace _\<path\>_ with the absolute path to `anywhere-configuration/samples` directory.
3. Launch the project in debug mode using F5.

## Conclusion

For more information please check [wiki][1] and explore the source code! 

If you have a suggestion or found an issue please consider [reporting it][2].

[1]: https://github.com/coherentsolutionsinc/anywhere-configuration/wiki
[2]: https://github.com/coherentsolutionsinc/anywhere-configuration/issues
