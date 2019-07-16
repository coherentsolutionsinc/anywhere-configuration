using System.Collections.Generic;
using System.Linq;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;
using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Moq;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationEnvironmentWithPostfixTests
    {
        [Fact]
        public void Should_get_value_of_item_with_postfix_When_called_GetValue_with_item_name_without_postfix()
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    ("name", "false"),
                    ("name_POSTFIX", "true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithPostfix(environment.Object, "POSTFIX");

            Assert.Equal("true", env.GetValue("name", s => (s, true)));
        }

        [Fact]
        public void Should_get_values_of_items_with_postfix_When_called_GetValues()
        {
            var environment = AnyWhereConfigurationEnvironmentMockFactory.CreateEnvironmentMock(
                new[]
                {
                    ("one", "one false"),
                    ("two", "two false"),
                    ("one_POSTFIX", "one true"),
                    ("two_POSTFIX", "two true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithPostfix(environment.Object, "POSTFIX");

            var values = env.GetValues().ToArray();
            Assert.Equal(2, values.Length);
            
            Assert.Equal("one", values[0].Key);
            Assert.Equal("one true", values[0].Value);

            Assert.Equal("two", values[1].Key);
            Assert.Equal("two true", values[1].Value);
        }
    }
}