namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataPath
    {
        public string Value { get; }

        public AnyWhereConfigurationDataPath(
            string path)
        {
            this.Value = path;
        }
    }
}