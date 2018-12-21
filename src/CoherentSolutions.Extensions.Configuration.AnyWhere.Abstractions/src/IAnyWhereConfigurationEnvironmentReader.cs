namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions
{
    public interface IAnyWhereConfigurationEnvironmentReader
    {
        IAnyWhereConfigurationEnvironment Environment { get; }

        string GetString(
            string name,
            string defaultValue = null,
            bool optional = false);

        bool GetBool(
            string name,
            bool defaultValue = false,
            bool optional = false);

        int GetInt(
            string name,
            int defaultValue = 0,
            bool optional = false);

        long GetLong(
            string name,
            long defaultValue = 0,
            bool optional = false);
    }
}