using System;
using System.IO;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public ref struct AnyWhereConfigurationPathEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        private readonly ReadOnlySpan<char> value;

        private AnyWhereConfigurationPath current;

        private ReadOnlySpan<char> inputValue;

        private State inputState;

        public AnyWhereConfigurationPathEnumerator(
            ReadOnlySpan<char> value)
        {
            this.value = value;

            this.current = default;

            this.inputValue = value;
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
                    this.inputState = State.Open;
                    goto case State.Open;
                case State.Open:
                    ReadOnlySpan<char> pathString;
                    for (;;)
                    {
                        var index = this.inputValue.IndexOf(Path.PathSeparator);
                        if (index >= 0)
                        {
                            this.inputValue = this.inputValue.TrimStart();
                            if (this.inputValue[0] == Path.PathSeparator)
                            {
                                this.inputValue = this.inputValue.Slice(1);
                                continue;
                            }
                        }

                        if (this.inputValue.IsEmpty || this.inputValue.IsWhiteSpace())
                        {
                            this.inputState = State.Closed;
                            return false;
                        }

                        if (index > 0)
                        {
                            this.inputValue = this.inputValue.Slice(0, index);
                        }
                        else
                        {
                            this.inputState = State.Closed;
                        }

                        pathString = this.inputValue;
                        break;
                    }

                    this.current = new AnyWhereConfigurationPath(pathString.ToString());
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

            this.inputValue = this.value;
            this.inputState = State.None;
        }
    }
}