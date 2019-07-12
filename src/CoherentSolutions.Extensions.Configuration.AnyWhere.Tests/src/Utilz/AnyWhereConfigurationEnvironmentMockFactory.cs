using System;
using System.Collections.Generic;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Utilz
{
    public static class AnyWhereConfigurationEnvironmentMockFactory
    {
        public static Mock<IAnyWhereConfigurationEnvironment> CreateEnvironmentMock(
            IEnumerable<(string key, string value)> values)
        {
            var environment = new Mock<IAnyWhereConfigurationEnvironment>();
            foreach (var (key, value) in values)
            {
                environment
                   .Setup(i => i.GetValue(key, It.IsAny<Func<string, (string value, bool converted)>>(), It.IsAny<string>(), It.IsAny<bool>()))
                   .Returns(value)
                   .Verifiable();
            }

            return environment;
        }
    }
}