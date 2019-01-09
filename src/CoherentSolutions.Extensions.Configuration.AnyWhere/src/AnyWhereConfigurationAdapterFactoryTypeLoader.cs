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

            var assemblyPath = this.assemblyLocator.FindAssembly(adapterArg.AdapterAssemblyName);
            if (assemblyPath != null)
            {
                var type =
                    Assembly.LoadFrom(assemblyPath)
                       .GetType(adapterArg.AdapterTypeName);

                if (!typeof(IAnyWhereConfigurationSourceAdapter).IsAssignableFrom(type))
                {
                    throw new InvalidOperationException(
                        $"{type.FullName} : type doesn't implement '{nameof(IAnyWhereConfigurationSourceAdapter)}'");
                }

                return type;
            }

            var sb = new StringBuilder()
               .AppendFormat("The assembly: '{0}' isn't found in any of probing paths:", adapterArg.AdapterAssemblyName)
               .AppendLine();

            foreach (var probingPath in this.assemblyLocator.GetProbingPaths())
            {
                sb.AppendLine("- ").Append(probingPath);
            }

            throw new TypeLoadException(sb.ToString());
        }
    }
}