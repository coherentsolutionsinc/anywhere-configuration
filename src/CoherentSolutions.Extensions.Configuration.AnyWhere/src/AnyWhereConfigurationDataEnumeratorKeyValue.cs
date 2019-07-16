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

        private AnyWhereConfigurationDataKeyValue current;

        private int inputLine;

        private int inputIndex;

        private string inputValue;

        private State inputState;

        public AnyWhereConfigurationDataKeyValueEnumerator(
            string inputValue)
        {
            this.current = default;

            this.inputLine = -1;
            this.inputIndex = 0;
            this.inputValue = inputValue;
            this.inputState = State.None;
        }

        public AnyWhereConfigurationDataKeyValue Current
        {
            get
            {
                if (this.inputState == State.None)
                {
                    throw new InvalidOperationException("The enumeration wasn't started.");
                }

                return this.current;
            }
        }

        public bool MoveNext()
        {
            switch (this.inputState)
            {
                case State.None:
                    if (string.IsNullOrWhiteSpace(this.inputValue))
                    {
                        this.inputState = State.Closed;
                        return false;
                    }

                    this.inputState = State.Open;
                    goto case State.Open;
                case State.Open:
                    var nl = Environment.NewLine.AsSpan();
                    for (;;)
                    {
                        this.inputLine++;

                        var input = this.inputValue.AsSpan(this.inputIndex);
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

                        if (index == (nl.Length - 1) || input.Length == index)
                        {
                            this.inputState = State.Closed;
                        }
                        else
                        {
                            this.inputIndex += index;
                        }

                        if (output.IsEmpty)
                        {
                            if (this.inputState == State.Closed)
                            {
                                return false;
                            }

                            continue;
                        }

                        index = output.IndexOf(KEY_VALUE_SEPARATOR_CHAR);
                        if (index < 0)
                        {
                            throw new InvalidOperationException(
                                $"Expected '{KEY_VALUE_SEPARATOR_CHAR}' at ({this.inputLine}, {ch})");
                        }
                        if (index == 0)
                        {
                            throw new InvalidOperationException(
                                $"Expected 'a key' before '{KEY_VALUE_SEPARATOR_CHAR}' at ({this.inputLine}, {ch})");
                        }
                        if (output.Length - 1 == 0)
                        {
                            throw new InvalidOperationException(
                                $"Expected 'a value' after '{KEY_VALUE_SEPARATOR_CHAR}' at ({this.inputLine}. {ch + 1})");
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

            this.inputLine = -1;
            this.inputIndex = 0;
            this.inputValue = null;
            this.inputState = State.None;
        }
    }
}