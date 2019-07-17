namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationFile
    {
        string Name { get; }

        string Directory { get; }

        string Path { get; }

        string GetContentAsString();
    }
}