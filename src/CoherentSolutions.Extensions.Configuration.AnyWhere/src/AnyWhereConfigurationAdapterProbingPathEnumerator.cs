using System;

using CoherentSolutions.Extensions.Configuration.AnyWhere.Abstractions;

namespace CoherentSolutions.Extensions.Configuration.AnyWhere
{
    public struct AnyWhereConfigurationAdapterProbingPathEnumerator
    {
        private enum State
        {
            None = 0,

            Open = 1,

            Closed = 2
        }

        public const string PROBING_PATH_PARAMETER_NAME = "PROBING_PATH";

        private readonly IAnyWhereConfigurationEnvironment environment;

        private AnyWhereConfigurationDataPathEnumerator enumerator;

        private State currentState;

        public AnyWhereConfigurationAdapterProbingPathEnumerator(
            IAnyWhereConfigurationEnvironment environment)
        {
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
         
            this.enumerator = default;
            this.currentState = State.None;
        }

        public AnyWhereConfigurationDataPath Current => this.enumerator.Current;

        public bool MoveNext()
        {
            switch (this.currentState)
            {
                case State.None:
                    var env = new AnyWhereConfigurationEnvironmentReader(this.environment);

                    this.enumerator = new AnyWhereConfigurationDataPathEnumerator(env.GetString(PROBING_PATH_PARAMETER_NAME, optional: true));
                    this.currentState = State.Open;
                    goto case State.Open;
                case State.Open:
                    if (this.enumerator.MoveNext())
                    {
                        return true;
                    }

                    this.currentState = State.Closed;
                    return false;
                case State.Closed:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset()
        {
            this.enumerator = default;
            this.currentState = State.None;
        }
    }
}