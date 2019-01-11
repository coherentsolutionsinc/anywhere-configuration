using System;
using System.Reflection;
using System.Runtime.Loader;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly IAnyWhereConfigurationAdapterAssemblyLocator assemblyLocator;

        public AnyWhereConfigurationAdapterAssemblyLoadContext(
            IAnyWhereConfigurationAdapterAssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator ?? throw new ArgumentNullException(nameof(assemblyLocator));
        }

        protected override Assembly Load(
            AssemblyName assemblyName)
        {
            try
            {
                // We always prefer assemblies from the default context over the adapter assembly dependency.
                return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
            }
            catch
            {
                // There is no 'try' overload for assembly loading no assembly can be loaded then we need
                // to use **assemblyLocator**
                var candidateAssemblyPath = this.assemblyLocator.FindAssembly(assemblyName.Name);
                if (candidateAssemblyPath == null)
                {
                    return null;
                }

                var candidateAssemblyName = AssemblyLoadContext.GetAssemblyName(candidateAssemblyPath);
                if (!AssemblyName.ReferenceMatchesDefinition(assemblyName, candidateAssemblyName))
                {
                    return null;
                }

                return this.LoadFromAssemblyPath(candidateAssemblyPath);
            }
        }
    }
}