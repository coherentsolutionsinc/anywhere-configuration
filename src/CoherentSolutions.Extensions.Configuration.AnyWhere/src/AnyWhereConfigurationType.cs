using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationType : IAnyWhereConfigurationType
    {
        private readonly Type type;

        public AnyWhereConfigurationType(
            Type type)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public object CreateInstance()
        {
            return Activator.CreateInstance(this.type);
        }
    }
}