using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationEnvironmentReader : IAnyWhereConfigurationEnvironmentReader
    {
        public IAnyWhereConfigurationEnvironment Environment { get; }

        public AnyWhereConfigurationEnvironmentReader(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public string GetString(
            string name,
            string defaultValue = null,
            bool optional = false)
        {
            return this.Environment.GetValue(
                name,
                s => (s, true),
                defaultValue,
                optional);
        }

        public bool GetBool(
            string name,
            bool defaultValue = false,
            bool optional = false)
        {
            return this.Environment.GetValue(
                name,
                s =>
                {
                    var converted = bool.TryParse(s, out var value);
                    return (value, converted);
                },
                defaultValue,
                optional);
        }

        public int GetInt(
            string name,
            int defaultValue = 0,
            bool optional = false)
        {
            return this.Environment.GetValue(
                name,
                s =>
                {
                    var converted = int.TryParse(s, out var value);
                    return (value, converted);
                },
                defaultValue,
                optional);
        }

        public long GetLong(
            string name,
            long defaultValue = 0,
            bool optional = false)
        {
            return this.Environment.GetValue(
                name,
                s =>
                {
                    var converted = long.TryParse(s, out var value);
                    return (value, converted);
                },
                defaultValue,
                optional);
        }
    }
}