namespace AbilityNew.Diagnostics
{
    public interface IAbilityTraceWriter
    {
            void Write(in AbilityTraceRecord record);
    }
}