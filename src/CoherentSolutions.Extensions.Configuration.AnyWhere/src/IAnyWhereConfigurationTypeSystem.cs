namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationTypeSystem
    {
        IAnyWhereConfigurationType Get(
            IAnyWhereConfigurationFile assembly,
            string name);
    }
}