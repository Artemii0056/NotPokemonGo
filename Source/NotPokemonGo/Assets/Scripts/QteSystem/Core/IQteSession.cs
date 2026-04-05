using System;

namespace QteSystem.Core
{
    public interface IQteSession : IDisposable
    {
        event Action<QteResult> Completed;
        bool IsCompleted { get; }
        QteResult? Result { get; }
    }
}