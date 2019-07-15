namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationFileSystem
    {
        bool FileExists(
            string path);
    }
}