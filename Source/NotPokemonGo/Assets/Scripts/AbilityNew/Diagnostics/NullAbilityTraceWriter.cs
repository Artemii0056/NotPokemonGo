namespace AbilityNew.Diagnostics
{
    public sealed class NullAbilityTraceWriter : IAbilityTraceWriter
    {
        public void Write(in AbilityTraceRecord record) { }
    }
}