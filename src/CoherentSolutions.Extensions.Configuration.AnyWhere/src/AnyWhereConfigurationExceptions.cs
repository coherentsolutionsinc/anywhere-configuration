using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public static class AnyWhereConfigurationExceptions
    {
        public static string GetEmptySearchResultsMessage(
            string path,
            IEnumerable<string> directories)
        {
            var sb = new StringBuilder()
               .AppendFormat("Cannot find '{0}' adapter assembly in any of probing paths:", path)
               .AppendLine();

            foreach (var directory in directories)
            {
                sb.Append("- ").AppendLine(directory);
            }

            return sb.ToString();
        }

        public static string GetAmbiguousSearchResultsMessage(
            IEnumerable<IAnyWhereConfigurationFileSearchResult> results)
        {
            var sb = new StringBuilder()
               .AppendLine("Found adapter related files in multiple directories:");

            foreach (var result in results)
            {
                sb.Append("- ").AppendLine(result.Directory);
                foreach (var file in result.Files.Where(i => i != null))
                {
                    sb.Append(" + ").AppendLine(file.Name);
                }
            }
            return sb.ToString();
        }

        public static string GetBadEnvironmentConfigurationMessage(
            string path)
        {
            return $"Unable to load environment configuration from '{path}'. Please see inner exception for details.";
        }

        public static string GetBadConfigurationInjectionMessage(
            AnyWhereConfigurationAdapterDefinition adapterDefinition)
        {
            return $"Unable to inject configuration from ({adapterDefinition.TypeName}). Please see inner exception for details.";
        }
    }
}