using System;
using QteSystem.TestQte;

namespace QteSystem
{
    public interface IProvidesQteResult
    {
        event Action<QteResult> OnReached;
    }
}