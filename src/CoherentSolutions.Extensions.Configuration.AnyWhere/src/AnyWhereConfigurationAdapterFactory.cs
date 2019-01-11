using System;
using System.Linq;

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

        public IAnyWhereConfigurationAdapter Create(
            AnyWhereConfigurationAdapterArgument adapterArg)
        {
            return (IAnyWhereConfigurationAdapter) Activator.CreateInstance(this.typeLoader.Load(adapterArg));
        }
    }
}