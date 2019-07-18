using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterDefinition
    {
        public string TypeName { get; }

        public string AssemblyName { get; }

        public AnyWhereConfigurationAdapterDefinition(
            string typeName,
            string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));
            }

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(assemblyName));
            }

            this.TypeName = typeName;
            this.AssemblyName = assemblyName;
        }
    }
}