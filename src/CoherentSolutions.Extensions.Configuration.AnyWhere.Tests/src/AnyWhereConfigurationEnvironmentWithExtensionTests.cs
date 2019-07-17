using System.Linq;
using CoherentSolutions.Extensions.Configuration.AnyWhere.Tests.Tools;

using Xunit;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.Tests
{
    public class AnyWhereConfigurationEnvironmentWithExtensionTests
    {
        [Fact]
        public void Should_get_value_of_item_from_extension_When_called_GetValue_when_source_doesnt_contain_requested_item()
        {
            var source = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("name", "false")
                });
            var extension = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("extension", "true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithExtension(source, extension);

            Assert.Equal("true", env.GetValue("extension", s => (s, true)));
        }

        [Fact]
        public void Should_get_value_of_item_from_extension_When_called_GetValue_when_source_contains_requested_item()
        {
            var source = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("name", "false")
                });
            var extension = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("name", "true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithExtension(source, extension);

            Assert.Equal("true", env.GetValue("name", s => (s, true)));
        }

        [Fact]
        public void Should_get_values_of_items_from_extension_and_then_source_When_called_GetValues()
        {
            var source = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("name", "name false"),
                    ("source", "source true")
                });
            var extension = AnyWhereConfigurationEnvironmentMockFactory.Create(
                new[]
                {
                    ("name", "name true"),
                    ("extension", "extension true")
                });

            var env = new AnyWhereConfigurationEnvironmentWithExtension(source, extension);

            var values = env.GetValues().ToArray();
            Assert.Equal(3, values.Length);
            
            Assert.Equal("name", values[0].Key);
            Assert.Equal("name true", values[0].Value);

            Assert.Equal("extension", values[1].Key);
            Assert.Equal("extension true", values[1].Value);

            Assert.Equal("source", values[2].Key);
            Assert.Equal("source true", values[2].Value);
        }
    }
}