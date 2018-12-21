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

            var fileName = Path.ChangeExtension(name, ".dll");
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
            }

            var sb = new StringBuilder()
               .AppendFormat("The assembly: '{0}' doesn't exist in any of the provided locations:", name)
               .AppendLine();

            foreach (var location in locations)
            {
                sb.Append("- ").AppendLine(location);
            }

            throw new FileNotFoundException(sb.ToString());
        }
    }
}