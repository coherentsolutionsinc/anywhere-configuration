using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationEnvironmentWithPrefixTests
    {
        [Fact]
        public void Should_get_environment_values_When_value_is_prefixed()
        {
            var environment = new Mock<IAnyWhereConfigurationEnvironment>();
            environment
               .Setup(instance => instance.GetValues())
               .Returns(
                    new[]
                    {
                        new KeyValuePair<string, string>("value", "value"),
                        new KeyValuePair<string, string>("value1", "value"),
                        new KeyValuePair<string, string>("value2", "value"),
                        new KeyValuePair<string, string>("PREFIX_", "value"),
                        new KeyValuePair<string, string>("PREFIX_value", "value"),
                        new KeyValuePair<string, string>("PREFIX_value1", "value"),
                        new KeyValuePair<string, string>("PREFIX_value2", "value")
                    });

            var prefixed = new AnyWhereConfigurationEnvironmentWithPrefix(environment.Object, "PREFIX");

            var values = prefixed.GetValues().ToArray();
            Assert.Equal(3, values.Length);
            Assert.Equal("PREFIX_value", values[0].Key);
            Assert.Equal("PREFIX_value1", values[1].Key);
            Assert.Equal("PREFIX_value2", values[2].Key);
        }
    }
}