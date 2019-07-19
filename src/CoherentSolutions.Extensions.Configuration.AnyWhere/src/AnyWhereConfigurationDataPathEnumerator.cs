using System;
using System.IO;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationDataPathEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        private readonly string inputValue;

        private AnyWhereConfigurationDataPath current;

        private int currentIndex;

        private State currentState;

        public AnyWhereConfigurationDataPathEnumerator(
            in string inputValue)
        {
            this.inputValue = inputValue;

            this.current = default;
            this.currentIndex = 0;
            this.currentState = State.None;
        }

        public AnyWhereConfigurationDataPath Current
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
                    for (;;)
                    {
                        var input = this.inputValue.AsSpan(this.currentIndex);
                        var index = input.IndexOf(Path.PathSeparator);

                        var output = index >= 0
                            ? input.Slice(0, index)
                            : input;

                        output = output.Trim();

                        index++;

                        if (index == 0 || input.Length == index)
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

                        this.current = new AnyWhereConfigurationDataPath(output.ToString());
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
            this.currentIndex = 0;
            this.currentState = State.None;
        }
    }
}