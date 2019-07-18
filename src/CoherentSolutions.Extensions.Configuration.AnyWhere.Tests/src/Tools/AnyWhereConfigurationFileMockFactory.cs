using System.IO;

using Moq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools
{
    public static class AnyWhereConfigurationFileMockFactory
    {
        public static IAnyWhereConfigurationFile Create(
            string directory,
            string name,
            string content)
        {
            var file = new Mock<IAnyWhereConfigurationFile>();
            file
               .Setup(instance => instance.Name)
               .Returns(name);
            file
               .Setup(instance => instance.Directory)
               .Returns(directory);
            file
               .Setup(instance => instance.Path)
               .Returns(Path.Combine(directory, name));
            file
               .Setup(instance => instance
                   .GetContentAsString())
               .Returns(content);

            return file.Object;
        }
    }
}