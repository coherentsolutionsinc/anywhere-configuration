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

        private const int MAX_STACK_ALLOC_LENGTH = 56;

        private const char SECRETS_SEPARATOR_CHAR = ';';

        private const char SECRET_ESCAPE_CHAR = '`';

        private const char SECRET_VERSION_SEPARATOR_CHAR = ':';

        private const char SECRET_ALIAS_SEPARATOR_CHAR = '/';

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

                        var ch = 0;
                        var ln = output.Length;

                        output = output.TrimStart();
                        if (ln > output.Length)
                        {
                            ch += ln - output.Length;
                        }

                        output = output.TrimEnd();

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

                        var relIndex = index + ch;

                        string name;
                        string alias;
                        string version;

                        index = LastIndexOf(output, SECRET_VERSION_SEPARATOR_CHAR);
                        if (index >= 0)
                        {
                            version = ToString(output.Slice(index + 1).Trim());
                            if (string.IsNullOrEmpty(version))
                            {
                                throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                                    $"Unexpected name-version separator '{SECRET_VERSION_SEPARATOR_CHAR}'.", 0, relIndex + index);
                            }

                            output = output.Slice(0, index);
                        }
                        else
                        {
                            version = string.Empty;
                        }

                        index = LastIndexOf(output, SECRET_ALIAS_SEPARATOR_CHAR);
                        if (index >= 0)
                        {
                            alias = ToString(output.Slice(index + 1).Trim());
                            if (string.IsNullOrEmpty(alias))
                            {
                                throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                                    $"Unexpected name-alias separator '{SECRET_ALIAS_SEPARATOR_CHAR}'.", 0, relIndex + index);
                            }

                            output = output.Slice(0, index);
                        }
                        else
                        {
                            alias = string.Empty;
                        }

                        name = ToString(output.Trim());
                        if (string.IsNullOrEmpty(name))
                        {
                            throw new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecretParsingException(
                                "Expected <name>.", 0, relIndex);
                        }

                        this.current = new AnyWhereAzureKeyVaultConfigurationSourceAdapterSecret(name, alias, version);
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

        private static int LastIndexOf(
            in ReadOnlySpan<char> input,
            char value)
        {
            for (var i = input.Length - 1; i >= 0; --i)
            {
                var c = input[i];
                if (value != c)
                {
                    continue;
                }

                if (i > 0 && (value == SECRET_VERSION_SEPARATOR_CHAR || value == SECRET_ALIAS_SEPARATOR_CHAR))
                {
                    for (int j = i - 1, count = 0; j >= 0; --j, ++count)
                    {
                        if (input[j] == SECRET_ESCAPE_CHAR)
                        {
                            continue;
                        }

                        if (count % 2 == 0)
                        {
                            return i;
                        }

                        break;
                    }
                }
                else
                {
                    return i;
                }
            }

            return -1;
        }

        private static string ToString(
            in ReadOnlySpan<char> input)
        {
            var value = input.Length < MAX_STACK_ALLOC_LENGTH
                ? stackalloc char[input.Length]
                : new char[input.Length];

            var j = 0;
            var i = 0;
            for (; i < input.Length;)
            {
                if (input[i] == SECRET_ESCAPE_CHAR && i < input.Length - 1)
                {
                    var c = input[i + 1];
                    if (c == SECRET_ESCAPE_CHAR || c == SECRET_ALIAS_SEPARATOR_CHAR || c == SECRET_VERSION_SEPARATOR_CHAR)
                    {
                        ++i;
                    }
                }

                value[j++] = input[i++];
            }

            return value.Slice(0, j).ToString();
        }
    }
}