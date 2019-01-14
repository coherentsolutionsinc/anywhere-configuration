# CoherentSolutions.Extensions.Configuration.AnyWhere

| Engine | Adapters List |
|:---:|:---:|
|[![nuget package](https://img.shields.io/badge/nuget-1.0.2-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere/)|[![nuget package](https://img.shields.io/badge/nuget-1.0.0-blue.svg)](https://www.nuget.org/packages/CoherentSolutions.Extensions.Configuration.AnyWhere.AdapterList/)

## About the project

**CoherentSolutions.Extensions.Configuration.AnyWhere** is an extension to [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration). This extension allows application to configure it's configuration sources using environment variables. 

### How it works?

The **CoherentSolutions.Extensions.Configuration.AnyWhere** is made of two parts: _configuration engine_ and _configuration adapter_.

**Configuration engine** is configured once in the application code as additional configuration source ([IConfigurationSource](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationsource?view=aspnetcore-2.2)) using `AddAnyWhereConfiguration` method. After registration configuration engine is responsible for reading required values from environment variables and load all of the requested configuration sources.

``` csharp
WebHost.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration(
    config =>
    {
      config.AddAnyWhereConfiguration();
    })
```

**Configuration adapter** is a "bridge" between configuration engine and configuration source. In code configuration adapter is represented by `IAnyWhereConfigurationAdapter` interface:

``` csharp
public interface IAnyWhereConfigurationAdapter
{
  void ConfigureAppConfiguration(
    IConfigurationBuilder configurationBuilder,
    IAnyWhereConfigurationEnvironmentReader environmentReader);
}
```

> Usually but not mandatory configuration adapter is implemented in the separate assembly.

Coupling between configuration engine and configuration adapters is done using special environment variables. All variables can be divided into two categories: **GLOBAL** and **LOCAL**.

**GLOBAL** variables are consumed only by configuration engine. They have the following format: **ANYWHERE_ADAPTER_GLOBAL_\{VARIABLE_NAME\}**:

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

Imagine a simplest ASP.NET Core application where entry point is configured as following:

``` csharp
WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .Build()
  .Run();
```

Application is build into container and deployed into local _development environment_ and _staging Kubernetes cluster hosted using Azure Kubernetes Services_.

In _development environment_ application consumes secrets from shared `.json` configuration file (because everyone trust everyone).

In contrary to _development environment_ in _staging environment_ application secrets are consumed from Azure Key Vault (because no on trust staging).

So the full code snippet is:

``` csharp
WebHost.CreateDefaultBuilder(args)   
  .UseStartup&lt;Startup>()
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

While this works it requires from the developers to modify startup code each time something is changed. More flexibility can be achieved when using **CoherentSolutions.Extensions.Configuration.AnyWhere**.

#### Updating application code

Using **CoherentSolutions.Extensions.Configuration.AnyWhere** required application entry point to be updated:

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

> It is important to understand you can use **CoherentSolutions.Extensions.Configuration.AnyWhere** in combination with other configuration sources.

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
