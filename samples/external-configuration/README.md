# External Configuration

This sample is a tiny console application that consumes configuration from `environment variables`. The 'pearl' is that instead of specifying configuration prefix using `ANYWHERE_ADAPTER_0_PREFIX` environment variable it uses a `CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.anywhere` configuration file (located in the /_adapters/EnvironmentVariables directory) to setup the prefix.

## What is inside?

**run-from-environment.cmd**

The command file that configures environment variables to use _EnvironmentVariables_ configuration adapter from `Code/_adapters/EnvironmentVariables`. Anywhere _Configuration Engine_ automatically reads `CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.anywhere` configuration file and configures _EnvironmentVariables_ configuration adapter to load only environment variables with _SECRET_ prefix.

The successful command execution results in the: 

> _VALUE = This secret value is obtained from the ENVIRONMENT VARIABLES

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

**/_adapters/EnvironmentVariables/CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.anywhere**

The `.anywhere` configuration file contains single value `PREFIX=SECRET`. This value is automatically read by _configuration engine_ when _EnvironmentVariables_ adapter is loaded.

```
PREFIX=SECRET
```

## How to use?

**Visual Studio**

The following steps will configure the project to use __EnvironmentVariables__ configuration adapter from `Code/_adapters/_EnvironmentVariables_`.

1. Open `/Code/Properties/launchSettings.json` and uncomment the following lines:

    ``` javascript
    "workingDirectory": "<path>/anywhere-configuration/samples",
    "environmentVariables": {
      "ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH": "different-config-sources/Code/_adapters/EnvironmentVariables",
      "ANYWHERE_ADAPTER_0_TYPE_NAME": "CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables.AnyWhereEnvironmentVariablesConfigurationSourceAdapter",
      "ANYWHERE_ADAPTER_0_ASSEMBLY_NAME": "CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables"
    }
    ```

2. Replace _\<path\>_ with the absolute path to `anywhere-configuration/samples` directory.
3. Launch the project in debug mode using F5.

## Conclusion

For more information please check [wiki][1] and explore the source code! 

If you have a suggestion or found an issue please consider [reporting it][2].

[1]: https://github.com/coherentsolutionsinc/anywhere-configuration/wiki
[2]: https://github.com/coherentsolutionsinc/anywhere-configuration/issues
