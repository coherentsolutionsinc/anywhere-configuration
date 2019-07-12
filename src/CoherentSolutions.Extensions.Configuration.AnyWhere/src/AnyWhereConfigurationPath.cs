namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationPath
    {
        public string Value { get; }

        public AnyWhereConfigurationPath(
            string path)
        {
            this.Value = path;
        }
    }
}