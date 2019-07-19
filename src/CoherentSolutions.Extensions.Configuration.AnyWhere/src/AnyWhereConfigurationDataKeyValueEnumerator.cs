using System;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataKeyValueEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        private const char KEY_VALUE_SEPARATOR_CHAR = '=';

        private readonly string inputValue;

        private AnyWhereConfigurationDataKeyValue current;

        private int currentLine;

        private int currentIndex;

        private State currentState;

        public AnyWhereConfigurationDataKeyValueEnumerator(
            in string inputValue)
        {
            this.inputValue = inputValue;

            this.current = default;
            this.currentLine = -1;
            this.currentIndex = 0;
            this.currentState = State.None;
        }

        public AnyWhereConfigurationDataKeyValue Current
        {
            get
            {
                if (this.currentState == State.None)
                {
                    throw new InvalidOperationException("The enumeration wasn't started.");
                }

                return this.current;
            }
        }

        public bool MoveNext()
        {
            switch (this.currentState)
            {
                case State.None:
                    if (string.IsNullOrWhiteSpace(this.inputValue))
                    {
                        this.currentState = State.Closed;
                        return false;
                    }

                    this.currentState = State.Open;
                    goto case State.Open;
                case State.Open:
                    var nl = Environment.NewLine.AsSpan();
                    for (;;)
                    {
                        this.currentLine++;

                        var input = this.inputValue.AsSpan(this.currentIndex);
                        var index = input.IndexOf(nl);

                        var output = index >= 0
                            ? input.Slice(0, index)
                            : input;

                        var ch = 0;
                        var ln = output.Length;

                        output = output.TrimStart();
                        if (ln > output.Length)
                        {
                            ch += ln - output.Length;
                        }

                        output = output.TrimEnd();
                        index += nl.Length;

                        if (index == nl.Length - 1 || input.Length == index)
                        {
                            this.currentState = State.Closed;
                        }
                        else
                        {
                            this.currentIndex += index;
                        }

                        if (output.IsEmpty)
                        {
                            if (this.currentState == State.Closed)
                            {
                                return false;
                            }

                            continue;
                        }

                        index = output.IndexOf(KEY_VALUE_SEPARATOR_CHAR);
                        if (index < 0)
                        {
                            throw new AnyWhereConfigurationParseException(
                                $"Expected '{KEY_VALUE_SEPARATOR_CHAR}'.", this.currentLine, ch);
                        }

                        if (index == 0)
                        {
                            throw new AnyWhereConfigurationParseException(
                                $"Expected 'a key' before '{KEY_VALUE_SEPARATOR_CHAR}'", this.currentLine, ch);
                        }

                        if (output.Length - 1 == index)
                        {
                            throw new AnyWhereConfigurationParseException(
                                $"Expected 'a value' after '{KEY_VALUE_SEPARATOR_CHAR}'", this.currentLine, ch + index + 1);
                        }

                        this.current = new AnyWhereConfigurationDataKeyValue(
                            output.Slice(0, index).Trim().ToString(),
                            output.Slice(index + 1).Trim().ToString());

                        return true;
                    }
                case State.Closed:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset()
        {
            this.current = default;
            this.currentLine = -1;
            this.currentIndex = 0;
            this.currentState = State.None;
        }
    }
}