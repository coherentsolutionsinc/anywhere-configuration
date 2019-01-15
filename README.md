# CoherentSolutions.Extensions.Configuration.AnyWhere

| Engine | Adapters List |
|:---:|:---:|
|[![nuget package](https://img.shields.io/badge/nuget-1.0.2-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere/)|[![nuget package](https://img.shields.io/badge/nuget-1.0.0-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList/)

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

**GLOBAL** variables are consumed by configuration engine. They have the following format: **ANYWHERE_ADAPTER_GLOBAL_\{VARIABLE_NAME\}**:

* **ANYWHERE_ADAPTER_GLOBAL** - is a predefined prefix.
* **\{VARIABLE_NAME\}** - is a name of the variable.

Currently configuration engine supports the following **GLOBAL** variables:

* **PROBING_PATH** - is the list of paths (separated by [Path.PathSeparator](https://docs.microsoft.com/en-us/dotnet/api/system.io.path.pathseparator?view=netstandard-2.0)) to search for an adapter assembly (by default only _current directory_ is scanned).

> It should be noted that current directory is always scanned during assemblies lookup.

**LOCAL** variables are consumed by both configuration engine and configuration adapters. They have the following format:   **ANYWHERE_ADAPTER\_\{INDEX\}\_\{VARIABLE_NAME\}**:

* **ANYWHERE_ADAPTER** - is a predefined prefix.
* **\{INDEX\}** - is a zero based index of the adapter being configured.
* **\{VARIABLE_NAME\}** - is a name of the variable.

> When configuring configuration adapters it is critically to understand that configuration adapter's indexes should be sequential and start from 0.
> 
> Any space / a gap between indexes is treated as end of list and the rest of configuration is ignored.
> 
> Configuration engine identifies configuration adapter using two environment variables:

Configuration adapter is identified and loaded by configuration engine using two variables:

* **TYPE_NAME** - is the full type name of the configuration adapter's type.
* **ASSEMBLY_NAME** - is the name of the assembly file where configuration adapter type is implemented.

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
> The configuration adapter for `json` configuration is already implemented and available as [package](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.Json/) or [binaries](https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.2/CoherentSolutions.Extensions.Configuration.AnyWhere.Json-2.1.0.zip). 

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

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=\<assembly location\>
* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json.AnyWhereJsonConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.Json
* ANYWHERE_ADAPTER_0_PATH=\<configuration file location\>
* ANYWHERE_ADAPTER_0_OPTIONAL=false

The configuration adapter assembly should be placed either in _current directory_ or it's directory should be specified in **PROBING_PATH** variable (**GLOBAL** scope).

#### Consuming configuration in staging

The Kubernetes can be integrated with Azure Key Vault using [kubernetes-keyvault-flexvol](https://github.com/Azure/kubernetes-keyvault-flexvol) project. This way all requested secrets are downloaded to Kubernetes volume in `key-per-file` format.

The configuration for `key-per-file` format is already implemented ([Microsoft.Extensions.Configuration.KeyPerFile](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.KeyPerFile/)) so all we need to do is to make it available for configuration engine.

> **NOTE**
>
> The configuration adapter for `key-per-file` configuration is already implemented and available as [package](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile/) or [binaries](https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.2/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile-2.1.0.zip). 

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

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=\<assembly location\>
* ANYWHERE_ADAPTER_0_TYPE_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile.AnyWhereKeyPerFileConfigurationSourceAdapter
* ANYWHERE_ADAPTER_0_ASSEMBLY_NAME=CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile
* ANYWHERE_ADAPTER_0_DIRECTORY_PATH=\<volume mapping location\>
* ANYWHERE_ADAPTER_0_OPTIONAL=false

### Well-known configuration adapters

There are set of well known configuration adapters:

* Json - package and binary.
* EnvironmentVariables - package and binary.
* KeyPerFile - package and binary.

All these configuration adapters make use of **Microsoft.Extensions.Configuration.*** packages to create the configuration source translate parameters from environment variables to configuration source parameters.

These configuration adapters available in form of packages and binaries:

| Name | Package | Binary |
|:-----|:-------:|:------:|
| Json | [![package][100]][101] | [![binary][102]][103] |
| EnvironmentVariables | [![package][104]][105] | [![binary][106]][107] |
| KeyPerFile | [![package][108]][109] | [![binary][110]][111] |

[100]: https://img.shields.io/badge/nuget-2.1.0-blue.svg
[101]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.Json/
[102]: https://img.shields.io/badge/binary-2.1.0-brightgreen.svg
[103]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.2/CoherentSolutions.Extensions.Configuration.AnyWhere.Json-2.1.0.zip
[104]: https://img.shields.io/badge/nuget-2.1.0-blue.svg
[105]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables/
[106]: https://img.shields.io/badge/binary-2.1.0-brightgreen.svg
[107]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.2/CoherentSolutions.Extensions.Configuration.AnyWhere.EnvironmentVariables-2.1.0.zip
[108]: https://img.shields.io/badge/nuget-2.1.0-blue.svg
[109]: https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile/
[110]: https://img.shields.io/badge/binary-2.1.0-brightgreen.svg
[111]: https://github.com/coherentsolutionsinc/anywhere-configuration/releases/download/1.0.2/CoherentSolutions.Extensions.Configuration.AnyWhere.KeyPerFile-2.1.0.zip

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

> `AddAnyWhereConfigurationAdapterList` **must** be called **before** `AddAnyWhereConfiguration`.

This well-known configuration adapters can be configured in the simplified fashion. Instead of using **TYPE_NAME** and **ASSEMBLY_NAME** variables you can simply use **NAME** variable:

* ANYWHERE_ADAPTER_GLOBAL_PROBING_PATH=\<assembly location\>
* ANYWHERE_ADAPTER_0_NAME=Json
* ANYWHERE_ADAPTER_0_PATH=\<configuration location\>
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
