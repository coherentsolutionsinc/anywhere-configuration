# CoherentSolutions.Extensions.Configuration.AnyWhere

| Build & Tests | Engine | Adapters List |
|:---:|:---:|:---:|
|![build & test](https://codebuild.us-east-1.amazonaws.com/badges?uuid=eyJlbmNyeXB0ZWREYXRhIjoiMnNkK0VHRHprQWNmdHQwU0hVMU1DYXBFSnZ0WHhUcUxPcjZmemhOWkM5cXRqZXBidnpFRnJnalRnWHdTTzVkQXgzUUJFZUtqNTR3ZlpnMjd2K2NUa0RjPSIsIml2UGFyYW1ldGVyU3BlYyI6InVOcGYxcGxtYlRBazZCR2EiLCJtYXRlcmlhbFNldFNlcmlhbCI6MX0%3D&branch=master)|[![nuget package](https://img.shields.io/badge/nuget-1.0.3-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere/)|[![nuget package](https://img.shields.io/badge/nuget-1.1.1-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList/)

_For list of available configuration adapters please see [the list](https://github.com/coherentsolutionsinc/anywhere-configuration/blob/master/README.md#well-known-configuration-adapters)_

## About the project

**CoherentSolutions.Extensions.Configuration.AnyWhere** is an extension to [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration). This extension allows application to configure it's configuration sources using environment variables. 

### How it works?

The **CoherentSolutions.Extensions.Configuration.AnyWhere** is made of two parts: _configuration engine_ and _configuration adapter_.

**Configuration engine** is configured once in the application code using `AddAnyWhereConfiguration` method. The configuration engine then is responsible for reading required values from environment variables and load all of the requested configuration sources.

``` csharp
WebHost.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration(
    config =>
    {
      config.AddAnyWhereConfiguration();
    })
```

**Configuration adapter** is a "bridge" between configuration engine and configuration source. It is represented by `IAnyWhereConfigurationAdapter` interface:

``` csharp
public interface IAnyWhereConfigurationAdapter
{
  void ConfigureAppConfiguration(
    IConfigurationBuilder configurationBuilder,
    IAnyWhereConfigurationEnvironmentReader environmentReader);
}
```

> Usually but not mandatory configuration adapter is implemented in the separate assembly.

Coupling between configuration engine and configuration adapters is done using special environment variables. All variables can be divided into **GLOBAL** and **LOCAL**.

**GLOBAL** variables are consumed by configuration engine. They have the following format: **ANYWHERE\_ADAPTER\_GLOBAL_\{VARIABLE_NAME\}**:

* **ANYWHERE\_ADAPTER\_GLOBAL** - is a predefined prefix.
* **\{VARIABLE_NAME\}** - is a name of the variable.

Currently configuration engine supports the following **GLOBAL** variables:

* **PROBING\_PATH** - is the list of paths (separated by [Path.PathSeparator](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.pathseparator?view=netstandard-2.0)) to search for an adapter assembly (by default only _current directory_ is scanned).

> It should be noted that current directory is always scanned during assemblies lookup.

**LOCAL** variables are consumed by both configuration engine and configuration adapters. They have the following format:   **ANYWHERE\_ADAPTER\_\{INDEX\}\_\{VARIABLE_NAME\}**:

* **ANYWHERE\_ADAPTER** - is a predefined prefix.
* **\{INDEX\}** - is a zero based index of the adapter being configured.
* **\{VARIABLE\_NAME\}** - is a name of the variable.

> When configuring configuration adapters it is critically to understand that configuration adapter's indexes should be sequential and start from 0.
> 
> Any space / a gap between indexes is treated as end of list and the rest of configuration is ignored.

Configuration adapter is identified and loaded by configuration engine using two variables:

* **TYPE\_NAME** - is the full type name of the configuration adapter's type.
* **ASSEMBLY\_NAME** - is the name of the assembly file where configuration adapter type is implemented.

All additional parameters required by the underlying `IConfigurationSource` are passed using the **LOCAL** variables and can be consumed using supplied instance of `IAnyWhereConfigurationEnvironmentReader`.

### Where it can be used?

Imagine a simplest ASP.NET Core application with entry point configured as following:

``` csharp
WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .Build()
  .Run();
```

Application is build into container and deployed to _development environment_ (developers machine) and _staging_ (Kubernetes cluster in Azure).

In _development environment_ application consumes secrets from shared `.json` configuration file (friendly environment).

In contrary to _development environment_ in _staging environment_ application secrets are consumed from Azure Key Vault (hostile environment).

So the full code snippet is:

``` csharp
WebHost.CreateDefaultBuilder(args)   
  .UseStartup<Startup>()
  .ConfigureAppConfiguration(
     (ctx,config) =>
     {
       if (ctx.HostingEnvironment.IsDevelopment())
       {
         // Load values from shared .json configuration file
       }
       if (ctx.HostingEnvironment.IsStaging())
       {
         // Load values from Azure Key Vault
       }
     })
  .Build()
  .Run();
```

This works (and there is nothing bad in this approach). The downside of this is a requirement to modify startup code each time something is changed - new environment added, some critical parameters are changed etc.

A bit more flexibility can be achieved by using **CoherentSolutions.Extensions.Configuration.AnyWhere**.

#### Updating application

1. Add reference to <a href="https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere/">CoherentSolutions.Extensions.Configuration.AnyWhere</a> nuget package.
2. Update application entry point:
  ``` csharp
  WebHost.CreateDefaultBuilder(args)   
    .UseStartup<Startup>()
    .ConfigureAppConfiguration(
      (ctx,config) =>
      {
        config.AddAnyWhereConfiguration();
      })
    .Build()
    .Run();
  ```
  > You **can** use **CoherentSolutions.Extensions.Configuration.AnyWhere** in combination with other configuration sources.

#### Consuming configuration in development

The configuration for `.json` format is already implemented ([Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/)) so all we need to do is to make it available for configuration engine.

> **NOTE**
>
> The configuration adapter for `json` configuration is already implemented and available as [package](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.Json/) or [binaries](https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.Json-2.1.1.zip). 

Here is the code for the `.json` configuration adapter:

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

* ANYWHERE\_ADAPTER\_GLOBAL\_PROBING\_PATH=\<locations\>
* ANYWHERE\_ADAPTER\_0\_TYPE\_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter
* ANYWHERE\_ADAPTER\_0\_ASSEMBLY\_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json
* ANYWHERE\_ADAPTER\_0\_PATH=\<configuration file location\>
* ANYWHERE\_ADAPTER\_0\_OPTIONAL=false

The configuration adapter assembly should be placed either in _current directory_ or it's directory should be specified in **PROBING\_PATH** variable (**GLOBAL** scope).

#### Consuming configuration in staging

The Kubernetes can be integrated with Azure Key Vault using [kubernetes-keyvault-flexvol](https://github.com/Azure/kubernetes-keyvault-flexvol) project. This way all requested secrets are downloaded to Kubernetes volume in `key-per-file` format.

The configuration for `key-per-file` format is already implemented ([Microsoft.Extensions.Configuration.KeyPerFile](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.KeyPerFile/)) so all we need to do is to make it available for configuration engine.

> **NOTE**
>
> The configuration adapter for `key-per-file` configuration is already implemented and available as [package](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile/) or [binaries](https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile-2.1.1.zip). 

Here is the code for the `key-per-file` configuration adapter:

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

* ANYWHERE\_ADAPTER\_GLOBAL\_PROBING\_PATH=\<locations\>
* ANYWHERE\_ADAPTER\_0\_TYPE\_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter
* ANYWHERE\_ADAPTER\_0\_ASSEMBLY\_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
* ANYWHERE\_ADAPTER\_0\_DIRECTORY\_PATH=\<volume mapping location\>
* ANYWHERE\_ADAPTER\_0\_OPTIONAL=false

### Well-known configuration adapters

There are set of well known configuration adapters:

* [Json](https://github.com/coherentsolutionsinc/anywhere-configuration/wiki/Json-Adapter)
* [EnvironmentVariables](https://github.com/coherentsolutionsinc/anywhere-configuration/wiki/EnvironmentVariables-Adapter)
* [KeyPerFile](https://github.com/coherentsolutionsinc/anywhere-configuration/wiki/KeyPerFile-Adapter)
* [AzureKeyVault](https://github.com/coherentsolutionsinc/anywhere-configuration/wiki/AzureKeyVault-Adapter)

The `Json`, `EnvironmentVariables` and `KeyPerFile` configuration adapters make use of corresponding **Microsoft.Extensions.Configuration.*** packages to create the configuration source. The implementation translates parameters from environment variables to configuration source parameters.

The `AzureKeyVault` configuration adapter implements workflow for obtaining Secrets from Azure Key Vault in managed identity scenarios.

These configuration adapters available in form of packages and binaries:

| Name | Package | Binary |
|:-----|:-------:|:------:|
| Json | [![package][100]][101] | [![binary][102]][103] |
| EnvironmentVariables | [![package][104]][105] | [![binary][106]][107] |
| KeyPerFile | [![package][108]][109] | [![binary][110]][111] |
| AzureKeyVault | [![package][112]][113] | [![binary][114]][115] |

[100]: https://img.shields.io/badge/nuget-2.1.1-blue.svg
[101]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.Json/
[102]: https://img.shields.io/badge/binary-2.1.1-brightgreen.svg
[103]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.Json-2.1.1.zip
[104]: https://img.shields.io/badge/nuget-2.1.1-blue.svg
[105]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables/
[106]: https://img.shields.io/badge/binary-2.1.1-brightgreen.svg
[107]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables-2.1.1.zip
[108]: https://img.shields.io/badge/nuget-2.1.1-blue.svg
[109]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile/
[110]: https://img.shields.io/badge/binary-2.1.1-brightgreen.svg
[111]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile-2.1.1.zip
[112]: https://img.shields.io/badge/nuget-2.1.1-blue.svg
[113]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault/
[114]: https://img.shields.io/badge/binary-2.1.1-brightgreen.svg
[115]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.3/CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault-2.1.1.zip

Well known configuration adapters are defined in [CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList) package and configured using `AddAnyWhereConfigurationAdapterList` method:

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

> **NOTE**
>
> `AddAnyWhereConfigurationAdapterList` **must** be called **before** `AddAnyWhereConfiguration`.

These well-known configuration adapters can be configured in the simplified fashion. Instead of using **TYPE\_NAME** and **ASSEMBLY\_NAME** variables you can simply use **NAME** variable:

* ANYWHERE\_ADAPTER\_GLOBAL\_PROBING\_PATH=\<locations\>
* ANYWHERE\_ADAPTER\_0\_NAME=Json
* ANYWHERE\_ADAPTER\_0\_PATH=\<configuration location\>
* ANYWHERE\_ADAPTER\_0\_OPTIONAL=false

### Conclusion

That is it :)

## Contributing

For additional information on how to contribute to this project, please see [CONTRIBUTING.md][3].

## Authors

This project is owned and maintained by [Coherent Solutions][4].

## License

This project is licensed under the MIT License - see the [LICENSE.md][5] for details.

## Third-Party Notice

The `AzureKeyVault` configuration adapter uses [Polly][6] for request-retry strategy implementation.

[3]:  CONTRIBUTING.md "Contributing"
[4]:  https://www.coherentsolutions.com/ "Coherent Solutions Inc."
[5]:  LICENSE.md "License"
[6]:  https://github.com/App-vNext/Polly "Polly"
