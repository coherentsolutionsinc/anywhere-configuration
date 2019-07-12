using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationPathEnumerable
    {
        private readonly string pathString;

        public AnyWhereConfigurationPathEnumerable(
            string pathString)
        {
            this.pathString = pathString;
        }

        public AnyWhereConfigurationPathEnumerator GetEnumerator()
        {
            return string.IsNullOrWhiteSpace(this.pathString)
                ? default
                : new AnyWhereConfigurationPathEnumerator(this.pathString.AsSpan());
        }
    }
}