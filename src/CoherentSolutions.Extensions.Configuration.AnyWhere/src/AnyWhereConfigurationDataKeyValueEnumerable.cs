namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataKeyValueEnumerable
    {
        private readonly string inputValue;

        public AnyWhereConfigurationDataKeyValueEnumerable(
            in string inputValue)
        {
            this.inputValue = inputValue;
        }

        public AnyWhereConfigurationDataKeyValueEnumerator GetEnumerator()
        {
            return new AnyWhereConfigurationDataKeyValueEnumerator(this.inputValue);
        }
    }
}