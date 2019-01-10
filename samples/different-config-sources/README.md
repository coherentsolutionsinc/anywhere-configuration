# Different Configuration Sources

This sample demonstrates a minimalistic console application that consumes configuration from `.json` configuration file or from directory using `key-per-file` approach.

## What is inside?

**Code.csproj**

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

TODO

## Conclusion

TODO