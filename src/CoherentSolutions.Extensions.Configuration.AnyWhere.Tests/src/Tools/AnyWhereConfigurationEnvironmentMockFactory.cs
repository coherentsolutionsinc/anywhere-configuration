using System;
using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools
{
    public static class AnyWhereConfigurationEnvironmentMockFactory
    {
        public static IAnyWhereConfigurationEnvironment Create(
            params (string key, string value)[] values)
        {
            return Create((IEnumerable<(string key, string value)>) values);
        }

        public static IAnyWhereConfigurationEnvironment Create(
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

            environment
               .Setup(i => i.GetValues())
               .Returns(values.Select(kv => new KeyValuePair<string, string>(kv.key, kv.value)));

            return environment.Object;
        }
    }
}