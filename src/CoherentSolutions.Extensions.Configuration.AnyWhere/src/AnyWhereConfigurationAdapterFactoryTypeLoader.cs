using System;
using System.IO;
using System.Reflection;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterFactoryTypeLoader : IAnyWhereConfigurationAdapterFactoryTypeLoader
    {
        private readonly IAnyWhereConfigurationAdapterAssemblyLocator assemblyLocator;

        public AnyWhereConfigurationAdapterFactoryTypeLoader(
            IAnyWhereConfigurationAdapterAssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator ?? throw new ArgumentNullException(nameof(assemblyLocator));
        }

        public Type Load(
            AnyWhereConfigurationAdapterArgument adapterArg)
        {
            if (string.IsNullOrEmpty(adapterArg.AdapterAssemblyName))
            {
                throw new ArgumentNullException(nameof(adapterArg.AdapterAssemblyName));
            }

            if (string.IsNullOrEmpty(adapterArg.AdapterTypeName))
            {
                throw new ArgumentNullException(nameof(adapterArg.AdapterAssemblyName));
            }

            var assemblyLoadContext = new AnyWhereConfigurationAdapterAssemblyLoadContext(this.assemblyLocator);
            var assemblyName = new AssemblyName(adapterArg.AdapterAssemblyName);

            try
            {
                var assembly = assemblyLoadContext.LoadFromAssemblyName(assemblyName);
                var type = assembly.GetType(adapterArg.AdapterTypeName, true);

                if (!typeof(IAnyWhereConfigurationAdapter).IsAssignableFrom(type))
                {
                    throw new InvalidOperationException(
                        $"{type.FullName} : type doesn't implement '{nameof(IAnyWhereConfigurationAdapter)}'");
                }

                return type;
            }
            catch (FileNotFoundException e)
            {
                var sb = new StringBuilder()
                   .AppendFormat("The assembly: '{0}' isn't found in any of probing paths:", adapterArg.AdapterAssemblyName)
                   .AppendLine();

                foreach (var probingPath in this.assemblyLocator.GetProbingPaths())
                {
                    sb.AppendLine("- ").Append(probingPath);
                }

                throw new FileNotFoundException(sb.ToString(), e.FileName, e);
            }
        }
    }
}