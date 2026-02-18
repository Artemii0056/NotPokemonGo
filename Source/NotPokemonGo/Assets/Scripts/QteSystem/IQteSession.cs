using System;
using QteSystem.TestQTE;

namespace QteSystem
{
    public interface IQteSession : IDisposable
    {
        event Action<QteResult> Completed;
        void Dispose();
        bool IsCompleted { get; }
        QteResult? Result { get; }
    }
}