using System;
using QteSystem.TestQTE;

namespace QteSystem
{
    public interface IProvidesQteResult
    {
        event Action<QteResult> OnReached;
    }
}