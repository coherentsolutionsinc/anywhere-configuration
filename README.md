# CoherentSolutions.Extensions.Configuration.AnyWhere

| Engine | Adapters List |
|:---:|:---:|
|[![nuget package](https://img.shields.io/badge/nuget-1.0.2-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere/)|[![nuget package](https://img.shields.io/badge/nuget-1.0.0-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList/)

## About the project

**CoherentSolutions.Extensions.Configuration.AnyWhere** is the extension to [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) that allows application to setup it's configuration sources using environment variables. 

### How it works?

The **CoherentSolutions.Extensions.Configuration.AnyWhere** consists from two parts - configuration engine and configuration adapters. 

**Configuration Engine** is configured once in the application source code as additional configuration source using `AddAnyWhereConfiguration` method. After registration configuration engine is responsible for reading required values from environment variables and load of all requested configuration sources.

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

**Configuration Adapter** is a "bridge" between configuration engine and configuration source. In code configuration adapter is represented by `IAnyWhereConfigurationAdapter` interface:

``` csharp
public interface IAnyWhereConfigurationAdapter
{
  void ConfigureAppConfiguration(
    IConfigurationBuilder configurationBuilder,
    IAnyWhereConfigurationEnvironmentReader environmentReader);
}
```

> Configuration adapter implementation is typically (but not mandatory) located in a separate assembly. 

What configuration adapters are used and what parameters they have are described using environment variables. All configuration engine recognizable environment variables can be divided into two categories: **GLOBAL** and **LOCAL**.

**GLOBAL** variables are consumed by configuration engine. They have the following format: **ANYWHERE_ADAPTER_GLOBAL_\{VARIABLE_NAME\}** where:

* **ANYWHERE_ADAPTER_GLOBAL** is a predefined prefix.
* **\{VARIABLE_NAME\}** is a name of the actual variable.

The engine defines the following **GLOBAL** variables:
* **PROBING_PATH** - the list of paths to search for an adapter assembly i.e. `/configuration-adapters` (the default is _current directory_).

> It should be noted that current directory is always scanned during assemblies lookup.

**LOCAL** variables are consumed by both configuration engine and configuration adapter. They have the following format: **ANYWHERE_ADAPTER_\{INDEX\}_\{VARIABLE_NAME\}** where:

* **ANYWHERE_ADAPTER** is a predefined prefix, only variables with this prefix are scanned by the engine.
* **\{INDEX\}** is a zero based index of the adapter being configured.
* **\{VARIABLE_NAME\}** is a name of the actual variable.

> When configuring multiple configuration adapters the indexes should be sequential i.e. 0, 1, 2, ..., N. Any gap between indexes is threated as end of adapters list and the rest of configuration is ignored.

Configuration engine identifies configuration adapter using two environment variables:

* **TYPE_NAME (required)** - the full type name of the configuration adapters type i.e. `CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter`.
* **ASSEMBLY_NAME (required)** - the name of the assembly where type is located i.e. `CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile`.

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

The application is packed into the container and should be deployable into local development environment and staging Kubernetes cluster. 
* When deployed to development application consumes secrets from the protected `.json` file.
* When deployed to staging application consumes secrets from  Azure Key Vault (ASK).

#### Consuming configuration in development

Here is the example configuration adapter implementation for consuming `.json` configuration:

``` csharp
// The code is taken from CoherentSolutions.Extensions.Configuration.AnyWhere.Json.dll
// The project has reference to CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions package
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

      // Adding Json configuration source and reading parameters from the environment
      configurationBuilder.AddJsonFile(
        environmentReader.GetString("PATH"),
        environmentReader.GetBool("OPTIONAL", optional: true),
        environmentReader.GetBool("RELOAD_ON_CHANGE", optional: true));
    }
  }
}
```

The environment variables are configured as following:

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=/adapters
* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json
* ANYWHERE_ADAPTER_0_PATH=/configuration/secrets.json
* ANYWHERE_ADAPTER_0_OPTIONAL=false

> The configuration adapter .dlls should be placed in /adapters directory prior to application execution.

#### Consuming configuration in staging

The Kubernetes can be integrated with AKS using the [flex-volume](https://github.com/Azure/kubernetes-keyvault-flexvol) in a way that all required secrets will be downloaded to kuberneters volume in _key-per-file_ format (let's imagine that volume is matched to **/configuration**).

The configuration adapter implementation is the following:

``` csharp
// The code is taken from CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.dll
// The project has reference to CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions package
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

The environment variables are configured as following:

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=/adapters
* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
* ANYWHERE_ADAPTER_0_DIRECTORY_PATH=/configuration
* ANYWHERE_ADAPTER_0_OPTIONAL=false

> The configuration adapter .dlls should be placed in /adapters directory prior to application execution.

### Adapters

There are set of configuration adapters already available as [nuget](https://www.nuget.org/profiles/coherentsolutions) packages and [binaries](https://github.com/coherentsolutionsinc/anywhere-configuration/releases):

* Json
* EnvironmentVariables
* KeyPerFile

The usage of available configuration adapters can be significantely simplified when using [CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList) nuget package:

``` csharp
WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .ConfigureAppConfiguration(
    config =>
    {
      config
        .AddAnyWhereConfigurationAdapterList()
        .AddAnyWhereConfiguration();
    })
  .Build()
  .Run();
```

Please note that `AddAnyWhereConfigurationAdapterList` **must** be called **before** `AddAnyWhereConfiguration` to take effect.

Now when well known adapter list is added we can configure configuration adapters from the list using **NAME** environment variable. For example the **Json** configuration adapter can be configured using the following environment variables:

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=/adapters
* ANYWHERE_ADAPTER_0_NAME=Json
* ANYWHERE_ADAPTER_0_PATH=/configuration/secrets.json
* ANYWHERE_ADAPTER_0_OPTIONAL=false

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
