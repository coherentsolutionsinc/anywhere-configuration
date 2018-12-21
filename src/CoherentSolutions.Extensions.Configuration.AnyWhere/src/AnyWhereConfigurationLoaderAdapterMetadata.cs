using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderAdapterMetadata
    {
        public string Name { get; }

        public string TypeName { get; }

        public string AssemblyName { get; }

        public AnyWhereConfigurationLoaderAdapterMetadata(
            string name,
            string typeName,
            string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));
            }

            if (string.IsNullOrWhiteSpace(assemblyName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(assemblyName));
            }

            this.Name = name;
            this.TypeName = typeName;
            this.AssemblyName = assemblyName;
        }
    }
}