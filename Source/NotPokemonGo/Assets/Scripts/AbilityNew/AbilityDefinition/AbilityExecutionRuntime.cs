using System.Threading;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionRuntime
    {
        public AbilityExecutionRuntime(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            Context = context;
            CancellationToken = cancellationToken;
            State = new AbilityExecutionState();
            Result = new AbilityExecutionResult();
        }
        
        public AbilityExecutionContext Context { get; }
        public AbilityExecutionState State { get; }
        public AbilityExecutionResult Result { get; }
        public CancellationToken CancellationToken { get; }
    }
}