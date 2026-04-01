using System.Threading;
using AbilityNew.Scripts;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionRuntime
    {
        public AbilityExecutionRuntime(AbilitySO abilitySo, AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            Context = context;
            CancellationToken = cancellationToken;
            Ability = abilitySo;
            State = new AbilityExecutionState();
            Result = new AbilityExecutionResult();
        }
        
        public AbilitySO Ability { get; }
        public AbilityExecutionContext Context { get; }
        public AbilityExecutionState State { get; }
        public AbilityExecutionResult Result { get; }
        public CancellationToken CancellationToken { get; }
    }
}