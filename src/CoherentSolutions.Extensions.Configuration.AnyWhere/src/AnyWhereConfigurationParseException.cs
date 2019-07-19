using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public class AnyWhereConfigurationParseException : Exception
    {
        public int Line { get; }

        public int Position { get; }

        public AnyWhereConfigurationParseException(
            string message,
            int line,
            int position)
            : base(CreateMessage(message, line, position))
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            if (line < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(line));
            }

            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            this.Line = line;
            this.Position = position;
        }

        private static string CreateMessage(
            string message,
            int line,
            int position)
        {
            return $"PARSE ERROR ({line}, {position}): {message}";
        }
    }
}