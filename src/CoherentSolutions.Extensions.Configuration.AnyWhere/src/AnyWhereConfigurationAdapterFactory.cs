using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterFactory : IAnyWhereConfigurationAdapterFactory
    {
        private readonly AnyWhereConfigurationAdapterFactoryTypeLoader typeLoader;

        public AnyWhereConfigurationAdapterFactory(
            AnyWhereConfigurationAdapterFactoryTypeLoader typeLoader)
        {
            this.typeLoader = typeLoader ?? throw new ArgumentNullException(nameof(typeLoader));
        }

        public IAnyWhereConfigurationSourceAdapter Create(
            AnyWhereConfigurationAdapterArgument adapterArg)
        {
            var type = this.typeLoader.Load(adapterArg);
            return (IAnyWhereConfigurationSourceAdapter) Activator.CreateInstance(type);
        }
    }
}