namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionRuntime
    {
        public AbilityExecutionRuntime(AbilityExecutionContext context)
        {
            Context = context;
            State = new AbilityExecutionState();
            Result = new AbilityExecutionResult();
        }
        
        public AbilityExecutionContext Context { get; }
        public AbilityExecutionState State { get; }
        public AbilityExecutionResult Result { get; }
    }
}