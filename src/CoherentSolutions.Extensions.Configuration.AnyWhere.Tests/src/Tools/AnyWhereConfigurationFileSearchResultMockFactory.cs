using System.Collections.Generic;
using System.Linq;

using Moq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools
{
    public static class AnyWhereConfigurationFileSearchResultMockFactory
    {
        public static IAnyWhereConfigurationFileSearchResult Create(
            string directory,
            params string[] names)
        {
            return Create(directory, (IEnumerable<string>)names);
        }

        public static IAnyWhereConfigurationFileSearchResult Create(
            string directory,
            IEnumerable<string> names)
        {
            return Create(directory, names.Select(n => (n, (string)null)));
        }

        public static IAnyWhereConfigurationFileSearchResult Create(
            string directory,
            params (string name, string content)[] tuples)
        {
            return Create(directory, (IEnumerable<(string name, string content)>)tuples);
        }

        public static IAnyWhereConfigurationFileSearchResult Create(
            string directory,
            IEnumerable<(string name, string content)> tuples)
        {
            var result = new Mock<IAnyWhereConfigurationFileSearchResult>();
            result
               .Setup(instance => instance.Directory)
               .Returns(directory);

            var files = new List<IAnyWhereConfigurationFile>();
            foreach (var (name, content) in tuples)
            {
                if (name is null)
                {
                    files.Add(null);
                    continue;
                }

                files.Add(AnyWhereConfigurationFileMockFactory.Create(directory, name, content));
            }

            result
               .Setup(instance => instance.Files)
               .Returns(files);

            return result.Object;
        }
    }
}