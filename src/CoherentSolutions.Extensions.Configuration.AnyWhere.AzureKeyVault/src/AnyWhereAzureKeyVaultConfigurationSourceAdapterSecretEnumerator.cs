using System;
using System.Runtime.ExceptionServices;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere.AzureKeyVault
{
    public ref struct AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator
    {
        private enum State
        {
            Open = 0,

            Closed = 1
        }

        private readonly ReadOnlySpan<char> value;

        private AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret current;

        private ReadOnlySpan<char> inputValue;

        private State inputState;

        private int inputIndex;

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretEnumerator(
            in ReadOnlySpan<char> value)
        {
            this.value = value;

            this.current = default;

            this.inputValue = value;
            this.inputState = State.Open;
            this.inputIndex = 0;
        }

        public bool MoveNext()
        {
            switch (this.inputState)
            {
                case State.Open:
                    ReadOnlySpan<char> secretString;
                    for (;;)
                    {
                        var index = this.inputValue.IndexOf(';');
                        if (index >= 0)
                        {
                            this.inputIndex += index;

                            this.inputValue = this.inputValue.TrimStart();
                            if (this.inputValue[0] == ';')
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

                        secretString = this.inputValue;
                        break;
                    }

                    try
                    {
                        this.current = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(secretString);
                    }
                    catch (AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException e)
                    {
                        throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                            e.Message,
                            this.value,
                            e.Position + this.inputIndex,
                            ExceptionDispatchInfo.Capture(e).SourceException);
                    }

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
            this.inputState = State.Open;
            this.inputIndex = 0;
        }

        public AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret Current => this.current;
    }
}