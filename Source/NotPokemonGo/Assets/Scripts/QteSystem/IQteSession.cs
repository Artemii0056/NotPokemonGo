using System;
using QteSystem.TestQte;

namespace QteSystem
{
    public interface IQteSession : IDisposable
    {
        event Action<QteResult> Completed;
        bool IsCompleted { get; }
        QteResult? Result { get; }
    }
}