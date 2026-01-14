using System;
using QTESystem.TestQTE;

namespace QTESystem
{
    public interface IQteSession : IDisposable
    {
        event Action<QteResult> Completed;

        bool IsCompleted { get; }
        QteResult? Result { get; }
    }
}