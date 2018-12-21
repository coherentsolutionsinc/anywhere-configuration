using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Microsoft.Extensions.Configuration;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoader
    {
        private readonly IAnyWhereConfigurationLoaderAdapterArgumentsReader adapterArgumentsReader;

        private readonly IAnyWhereConfigurationLoaderAssemblyLocator assemblyLocator;

        private readonly IAnyWhereConfigurationLoaderTypeLoader typeLoader;

        public AnyWhereConfigurationLoader(
            IAnyWhereConfigurationLoaderAdapterArgumentsReader adapterArgumentsReader,
            IAnyWhereConfigurationLoaderAssemblyLocator assemblyLocator,
            IAnyWhereConfigurationLoaderTypeLoader typeLoader)
        {
            this.adapterArgumentsReader = adapterArgumentsReader ?? throw new ArgumentNullException(nameof(adapterArgumentsReader));
            this.assemblyLocator = assemblyLocator ?? throw new ArgumentNullException(nameof(assemblyLocator));
            this.typeLoader = typeLoader ?? throw new ArgumentNullException(nameof(typeLoader));
        }

        public void ConfigureAppConfiguration(
            IConfigurationBuilder configurationBuilder,
            IAnyWhereConfigurationEnvironment environment)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            foreach (var adapterArg in this.adapterArgumentsReader.Read(environment))
            {
                try
                {
                    var adapter = this.ActivateAdapter(adapterArg);

                    adapter.ConfigureAppConfiguration(
                        configurationBuilder, 
                        adapterArg.AdapterEnvironmentReader);
                }
                catch (Exception exception)
                {
                    var arguments = adapterArg.AdapterEnvironmentReader.Environment.GetValues().ToDictionary(kv => kv.Key, kv => kv.Value);
                    throw new AnyWhereConfigurationLoaderException(
                        adapterArg.AdapterTypeName,
                        adapterArg.AdapterAssemblyName,
                        arguments,
                        exception.InnerException);
                }
            }
        }

        private IAnyWhereConfigurationSourceAdapter ActivateAdapter(
            AnyWhereConfigurationLoaderAdapterArgument adapterArg)
        {
            var assemblyPath = this.assemblyLocator.FindAssembly(
                new HashSet<string>(adapterArg.AdapterSearchPaths.Split(Path.PathSeparator)),
                adapterArg.AdapterAssemblyName);

            var type = this.typeLoader.FromAssembly(assemblyPath, adapterArg.AdapterTypeName);

            if (!typeof(IAnyWhereConfigurationSourceAdapter).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"The '{type.FullName}' doesn't implement '{typeof(IAnyWhereConfigurationSourceAdapter).FullName}'");
            }

            return (IAnyWhereConfigurationSourceAdapter) Activator.CreateInstance(type);
        }
    }
}