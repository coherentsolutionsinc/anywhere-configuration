# CoherentSolutions.Extensions.Configuration.AnyWhere


## About the project

**CoherentSolutions.Extensions.Configuration.AnyWhere** is the extension to [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) that allows application to setup it's own configurable configuration engine. The engine is configured using environment variables what allows it to move the responsibility of configuration source management from the application code to the environment configuration.

### How it works?

The **CoherentSolutions.Extensions.Configuration.AnyWhere** consists from two parts: the configuration engine and configuration adapters. The engine is initialized using `AddAnyWhereConfiguration` extention method:

``` csharp
WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .ConfigureAppConfiguration(
    config =>
    {
      config.AddAnyWhereConfiguration();
    })
  .Build()
  .Run();
```

When the engine is initialized the application is ready to consume configuration throgh the adapters. 

The adapter is represented by the implementation of `IAnyWhereConfigurationSourceAdapter` interface:

``` csharp
public interface IAnyWhereConfigurationSourceAdapter
{
  void ConfigureAppConfiguration(
    IConfigurationBuilder configurationBuilder,
    IAnyWhereConfigurationEnvironmentReader environmentReader);
}
```

The implementation typically (but not mandatory) is located in a separate assembly. 

The purpose of the adapter is to initialize add additional configuration source to `IConfigurationBuilder`. Adapter is picked up by the engine using the information stored in environment variables. All engine recognizable environment variables have the following format: **ANYWHERE_ADAPTER_\{INDEX\}_\{VARIABLE_NAME\}**:

* **ANYWHERE_ADAPTER** is a predefined prefix, only variables with this prefix are scanned by the engine.
* **\{INDEX\}** is a zero based index of the adapter being configured.
* **\{VARIABLE_NAME\}** is a name of the actual variable.

> When configuring multiple adapters the indexes should be sequential i.e. 0, 1, 2, ..., N. Any gap between indexes is threated as end of list and the rest of adapters will be ignored.

Adapter is identified and loaded using values of three variables:

* **TYPE_NAME (required)** - the full name of the adapters type i.e. `CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter`.
* **ASSEMBLY_NAME (required)** - the name of the assembly where type is located i.e. `CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile`.
* **SEARCH_PATH (optional)** - the list of paths to search for an assembly i.e. `/configuration-adapters` (the default is _current directory_).

All additional parameters required by the underlying `IConfigurationSource` are passed using the same mechanism and can be consumed using provided `IAnyWhereConfigurationEnvironmentReader`.

### Where it can be used?

Imagine a simple application with the entry point configured as following:
``` csharp
// The project has reference to CoherentSolutions.Extensions.Configuration.AnyWhere

WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .ConfigureAppConfiguration(
    config =>
    {
      config.AddAnyWhereConfiguration();
    })
  .Build()
  .Run();
```

The application is packed into container should be deployable into local development environment and staging Kubernetes cluster. 
* When deployed to development application consumes secrets from the protected `.json` file.
* When deployed to staging application consumes secrets from  Azure Key Vault (ASK).

#### Consuming configuration in development

Here is the example adapter implementation for consuming `.json` configuration:

``` csharp
// The code is taken from CoherentSolutions.Extensions.Configuration.AnyWhere.json.dll
// The project has reference to CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
// The project has reference to Microsoft.Extensions.Configuration.Json package

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Json
{
  public class AnyWhereJsonConfigurationSourceAdapter : IAnyWhereConfigurationSourceAdapter
  {
    public void ConfigureAppConfiguration(
      IConfigurationBuilder configurationBuilder,
      IAnyWhereConfigurationEnvironmentReader environmentReader)
    {
      if (configurationBuilder == null)
      {
        throw new ArgumentNullException(nameof(configurationBuilder));
      }

      if (environmentReader == null)
      {
        throw new ArgumentNullException(nameof(environmentReader));
      }

      configurationBuilder.AddJsonFile(
        environmentReader.GetString("PATH"),
        environmentReader.GetBool("OPTIONAL", optional: true),
        environmentReader.GetBool("RELOAD_ON_CHANGE", optional: true));
    }
  }
}
```

\* The **Json** adapter is already implemented and can be downloaded [here].

The environment variables are configured as following:

* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereKeyPerFileConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json
* ANYWHERE_ADAPTER_0_SEARCH_PATH=/adapters
* ANYWHERE_ADAPTER_0_PATH=/configuration/secrets.json
* ANYWHERE_ADAPTER_0_OPTIONAL=false

> The adapter .dlls should be placed in /adapters directory prior to engine execution.

#### Consuming configuration in staging

The Kubernetes can be integrated with AKS using the [flex-volume](https://github.com/Azure/kubernetes-keyvault-flexvol) in a way that all required secrets will be downloaded to this volume in _key-per-file_ format.

The adapter implementation is the following:

``` csharp
// The code is taken from CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.dll
// The project has reference to CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
// The project has reference to Microsoft.Extensions.Configuration.KeyPerFile package

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
{
  public class AnyWhereKeyPerFileConfigurationSourceAdapter : AnyWhereConfigurationSourceAdapter
  {
    public void ConfigureAppConfiguration(
      IConfigurationBuilder configurationBuilder,
      IAnyWhereConfigurationEnvironmentReader environmentReader)
    {
      if (configurationBuilder == null)
      {
        throw new ArgumentNullException(nameof(configurationBuilder));
      }

      if (environmentReader == null)
      {
        throw new ArgumentNullException(nameof(environmentReader));
      }

      configurationBuilder.AddKeyPerFile(
        environmentReader.GetString("DIRECTORY_PATH"),
        environmentReader.GetBool("OPTIONAL", optional: true));
    }
  }
}
```

\* The **KeyPerFile** adapter is already implemented and can be downloaded [here].

The environment variables are configured as following:

* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
* ANYWHERE_ADAPTER_0_SEARCH_PATH=/adapters
* ANYWHERE_ADAPTER_0_DIRECTORY_PATH=/configuration
* ANYWHERE_ADAPTER_0_OPTIONAL=false

> The adapter .dlls should be placed in /adapters directory prior to engine execution.

### Conclusion

That is it :)

## Contributing

For additional information on how to contribute to this project, please see [CONTRIBUTING.md][3].

## Authors

This project is owned and maintained by [Coherent Solutions][4].

## License

This project is licensed under the MS-PL License - see the [LICENSE.md][5] for details.

[3]:  CONTRIBUTING.md "Contributing"
[4]:  https://www.coherentsolutions.com/ "Coherent Solutions Inc."
[5]:  LICENSE.md "License"