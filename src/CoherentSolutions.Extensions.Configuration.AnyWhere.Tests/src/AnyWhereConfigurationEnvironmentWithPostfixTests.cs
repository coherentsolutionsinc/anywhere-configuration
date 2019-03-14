using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationEnvironmentWithPostfixTests
    {
        [Fact]
        public void Should_get_environment_values_When_value_is_postfixed()
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
                        new KeyValuePair<string, string>("_POSTFIX", "value"),
                        new KeyValuePair<string, string>("value_POSTFIX", "value"),
                        new KeyValuePair<string, string>("value1_POSTFIX", "value"),
                        new KeyValuePair<string, string>("value2_POSTFIX", "value")
                    });

            var prefixed = new AnyWhereConfigurationEnvironmentWithPostfix(environment.Object, "POSTFIX");

            var values = prefixed.GetValues().ToArray();
            Assert.Equal(3, values.Length);
            Assert.Equal("value_POSTFIX", values[0].Key);
            Assert.Equal("value1_POSTFIX", values[1].Key);
            Assert.Equal("value2_POSTFIX", values[2].Key);
        }
    }
}