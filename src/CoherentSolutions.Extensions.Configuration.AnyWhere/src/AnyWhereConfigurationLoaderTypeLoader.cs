using System;
using System.Reflection;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderTypeLoader : IAnyWhereConfigurationLoaderTypeLoader
    {
        public Type FromAssembly(
            string assemblyPath,
            string typeName)
        {
            return Assembly.LoadFile(assemblyPath).GetType(typeName, true);
        }
    }
}