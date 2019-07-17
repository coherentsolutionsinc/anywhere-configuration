namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public interface IAnyWhereConfigurationType
    {
        T CreateInstance<T>();
    }
}