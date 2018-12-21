using System;
using System.IO;
using System.Reflection;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderTypeLoader : IAnyWhereConfigurationLoaderTypeLoader
    {
        public Type FromAssembly(
            string assemblyPath,
            string typeName)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(assemblyPath));
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeName));
            }

            return Assembly.LoadFile(assemblyPath).GetType(typeName, true);
        }
    }
}