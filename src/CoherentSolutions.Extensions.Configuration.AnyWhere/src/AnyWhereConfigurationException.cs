using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationAdapterException : Exception
    {
    }

    public class AnyWhereConfigurationException : Exception
    {
        private const string UNKNOWN_ASSEMBLY_PATH = "<unknown>";

        private const string UNKNOWN_TYPE_NAME = "<unknown>";

        public string AssemblyPath { get; }

        public string TypeName { get; }

        public IReadOnlyDictionary<string, string> Arguments { get; }

        public AnyWhereConfigurationException(
            string typeName,
            string assemblyPath,
            IReadOnlyDictionary<string, string> arguments = null,
            Exception innerException = null)
            : base(CreateMessage(typeName, assemblyPath, arguments), innerException)
        {
            this.AssemblyPath = assemblyPath;
            this.TypeName = typeName;

            if (arguments != null)
            {
                this.Arguments = new ReadOnlyDictionary<string, string>(arguments.ToDictionary(kv => kv.Key, kv => kv.Value));
            }
        }

        private static string CreateMessage(
            string typeName,
            string assemblyPath,
            IReadOnlyDictionary<string, string> arguments)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                typeName = UNKNOWN_TYPE_NAME;
            }

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                assemblyPath = UNKNOWN_ASSEMBLY_PATH;
            }

            var stringBuilder = new StringBuilder()
               .AppendFormat("Cannot load adapter type '{0}' from assembly '{1}'.", typeName, assemblyPath)
               .AppendLine();

            if (arguments is null || arguments.Count == 0)
            {
                return stringBuilder.ToString();
            }

            stringBuilder
               .AppendFormat("The following arguments ({0} in total) were provided:", arguments.Count)
               .AppendLine();

            foreach (var kv in arguments)
            {
                stringBuilder
                   .AppendFormat("'{0}'='{1}'", kv.Key, kv.Value)
                   .AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}