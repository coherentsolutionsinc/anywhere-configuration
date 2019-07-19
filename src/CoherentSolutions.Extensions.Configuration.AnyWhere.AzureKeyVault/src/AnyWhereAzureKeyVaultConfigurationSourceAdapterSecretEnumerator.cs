using System;
using System.Runtime.ExceptionServices;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        private const char SECRETS_SEPARATOR_CHAR = ';';

        private readonly string inputValue;

        private AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret current;

        private int currentIndex;

        private State currentState;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret Current
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

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(
            in string value)
        {
            this.inputValue = value;

            this.current = default;
            this.currentIndex = 0;
            this.currentState = State.None;
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
                        var index = input.IndexOf(SECRETS_SEPARATOR_CHAR);

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

                        try
                        {
                            this.current = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(output);
                        }
                        catch (AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException e)
                        {
                            throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                                e.Message,
                                this.inputValue,
                                e.Position + this.currentIndex,
                                ExceptionDispatchInfo.Capture(e).SourceException);
                        }

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
            this.currentState = State.Open;
        }
    }
}