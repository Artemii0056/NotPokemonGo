using System;
using QTESystem.TestQTE;

namespace QTESystem
{
    public interface IProvidesQteResult
    {
        event Action<QteResult> OnReached;
    }
}