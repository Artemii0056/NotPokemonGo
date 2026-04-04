namespace AbilityNew.Diagnostics
{
    public  class AbilityTraceRecord
    {
        public string ExecutionId;
        public string AbilityName;
        public string SourceName;
        public string TargetName;

        public string StepName;
        public string ExecutorName;

        public AbilityTracePhase Phase;
        public AbilityTraceSource Source;

        public string Message;
        public long TimestampUtcTicks;
        public int Frame;
    }
}