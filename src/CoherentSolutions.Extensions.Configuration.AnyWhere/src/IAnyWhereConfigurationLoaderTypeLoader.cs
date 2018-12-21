using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationLoaderTypeLoader
    {
        Type FromAssembly(
            string assemblyPath,
            string typeName);
    }
}