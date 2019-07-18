using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public static class AnyWhereConfigurationExceptions
    {
        public static string EmptySearchResultsMessage(
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

        public static string AmbiguousSearchResultsMessage(
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

        public static string ErrorLoadingEnvironmentConfigurationMessage(
            string path)
        {
            return $"Error loading environment configuration from '{path}'. Please see inner exception for details.";
        }

        public static string ErrorLoadingConfigurationMessage(
            AnyWhereConfigurationAdapterDefinition adapterDefinition)
        {
            var sb = new StringBuilder()
               .AppendLine("Error loading configuration:")
               .Append("- ").AppendLine(adapterDefinition.AssemblyName)
               .Append("- ").AppendLine(adapterDefinition.TypeName)
               .AppendLine("Please see inner exception for more details.");

            return sb.ToString();
        }
    }
}