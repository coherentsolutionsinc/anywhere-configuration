using System;
using System.IO;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationPathEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        public const string PATH_PARAMETER_NAME = "PATH";

        private readonly IAnyWhereConfigurationEnvironment environment;

        private AnyWhereConfigurationPath current;

        private int inputIndex;
        
        private string inputValue;

        private State inputState;

        public AnyWhereConfigurationPathEnumerator(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));

            this.current = default;

            this.inputIndex = 0;
            this.inputValue = null;
            this.inputState = State.None;
        }

        public AnyWhereConfigurationPath Current
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
                    if (this.environment != null)
                    {
                        var env = new AnyWhereConfigurationEnvironmentReader(this.environment);
                        
                        this.inputValue = env.GetString(PATH_PARAMETER_NAME, optional: true);
                        if (this.inputValue != null)
                        {
                            this.inputState = State.Open;
                            goto case State.Open;
                        }
                    }
                    this.inputState = State.Closed;
                    return false;
                case State.Open:
                    ReadOnlySpan<char> output;
                    for (;;)
                    {
                        var input = this.inputValue.AsSpan(this.inputIndex);
                        var index = input.IndexOf(Path.PathSeparator);

                        output = index >= 0
                            ? input.Slice(0, index)
                            : input;

                        output = output.Trim();

                        index++;

                        if (index == 0 || input.Length == index)
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
                        break;
                    }

                    this.current = new AnyWhereConfigurationPath(output.ToString());
                    return true;
                case State.Closed:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset()
        {
            this.current = default;

            this.inputIndex = 0;
            this.inputValue = null;
            this.inputState = State.None;
        }
    }
}