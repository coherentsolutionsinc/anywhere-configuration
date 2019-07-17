using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationTypeSystem : IAnyWhereConfigurationTypeSystem
    {
        private class TypeSystemLoadContext : AssemblyLoadContext
        {
            private readonly string basePath;

            public TypeSystemLoadContext(
                string basePath)
            {
                if (string.IsNullOrWhiteSpace(basePath))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(basePath));
                }

                this.basePath = basePath;
            }

            protected override Assembly Load(
                AssemblyName assemblyName)
            {
                var path = Path.Combine(this.basePath, assemblyName.Name);
                
                var candidatePath = Path.ChangeExtension(path, ".exe");
                if (!File.Exists(candidatePath))
                {
                    candidatePath = Path.ChangeExtension(candidatePath, ".dll");
                    if (!File.Exists(candidatePath))
                    {
                        // If no file can be found for assembly, try to load assembly from default context.
                        return Default.LoadFromAssemblyName(assemblyName);
                    }
                }

                return !AssemblyName.ReferenceMatchesDefinition(assemblyName, GetAssemblyName(candidatePath))
                    ? null
                    : this.LoadFromAssemblyPath(candidatePath);
            }
        }

        public IAnyWhereConfigurationType Get(
            IAnyWhereConfigurationFile assembly,
            string name)
        {
            var lc = new TypeSystemLoadContext(assembly.Directory);
            try
            {
                return new AnyWhereConfigurationType(
                    lc.LoadFromAssemblyPath(assembly.Path).GetType(name, true));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    $"Could not load assembly '{assembly.Path}' or it's dependencies. Please see inner exception for details.",
                    e);
            }
        }
    }
}