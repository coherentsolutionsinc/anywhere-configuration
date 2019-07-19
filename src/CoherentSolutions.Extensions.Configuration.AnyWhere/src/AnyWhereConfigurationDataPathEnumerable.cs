namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataPathEnumerable
    {
        private readonly string inputValue;

        public AnyWhereConfigurationDataPathEnumerable(
            in string inputValue)
        {
            this.inputValue = inputValue;
        }

        public AnyWhereConfigurationDataPathEnumerator GetEnumerator()
        {
            return new AnyWhereConfigurationDataPathEnumerator(this.inputValue);
        }
    }
}