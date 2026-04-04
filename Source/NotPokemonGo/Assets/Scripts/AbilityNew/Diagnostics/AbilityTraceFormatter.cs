using System;

namespace AbilityNew.Diagnostics
{
    public static class AbilityTraceFormatter
    {
        public static string Format(in AbilityTraceRecord record)
        {
            string time = new DateTime(record.TimestampUtcTicks, DateTimeKind.Utc)
                .ToLocalTime()
                .ToString("HH:mm:ss.fff");

            return
                $"{time} | " +
                $"exec={record.ExecutionId} | " +
                $"ability={record.AbilityName} | " +
                $"src={record.SourceName} | " +
                $"target={record.TargetName} | " +
                $"step={record.StepName} | " +
                $"executor={record.ExecutorName} | " +
                $"phase={record.Phase} | " +
                $"source={record.Source} | " +
                $"frame={record.Frame} | " +
                $"{record.Message}";
        }
    }
}