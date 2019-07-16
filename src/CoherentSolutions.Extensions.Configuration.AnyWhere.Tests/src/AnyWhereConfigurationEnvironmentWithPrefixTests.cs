using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;
using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationEnvironmentWithPrefixTests
    {
        [Fact]
        public void Should_get_value_of_item_with_prefix_When_called_GetValue_with_item_name_without_prefix()
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    ("name", "false"),
                    ("PREFIX_name", "true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithPrefix(environment.Object, "PREFIX");

            Assert.Equal("true", env.GetValue("name", s => (s, true)));
        }

        [Fact]
        public void Should_get_values_of_items_with_prefix_When_called_GetValues()
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    ("one", "one false"),
                    ("two", "two false"),
                    ("PREFIX_one", "one true"),
                    ("PREFIX_two", "two true")
                });

            var prefixed = new AnyWhereConfigurationEnvironmentWithPrefix(environment.Object, "PREFIX");

            var values = prefixed.GetValues().ToArray();
            Assert.Equal(2, values.Length);
            
            Assert.Equal("one", values[0].Key);
            Assert.Equal("one true", values[0].Value);

            Assert.Equal("two", values[1].Key);
            Assert.Equal("two true", values[1].Value);
        }
    }
}