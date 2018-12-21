using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationLoaderAssemblyLocator : IAnyWhereConfigurationLoaderAssemblyLocator
    {
        public string FindAssembly(
            IReadOnlyCollection<string> locations,
            string name)
        {
            if (locations == null)
            {
                throw new ArgumentNullException(nameof(locations));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var fileName = $"{name}.dll";

            var resolvedLocations = new List<string>(locations.Count);
            foreach (var location in locations)
            {
                var path = Path.Combine(location, fileName);
                if (!Path.IsPathRooted(location))
                {
                    path = Path.GetFullPath(path);
                }

                if (File.Exists(path))
                {
                    return path;
                }

                resolvedLocations.Add(path);
            }

            var sb = new StringBuilder()
               .AppendFormat("The assembly: '{0}' doesn't exist in any of the provided locations:", name)
               .AppendLine();

            foreach (var resolvedLocation in resolvedLocations)
            {
                sb.Append("- ").AppendLine(resolvedLocation);
            }

            throw new FileNotFoundException(sb.ToString());
        }
    }
}