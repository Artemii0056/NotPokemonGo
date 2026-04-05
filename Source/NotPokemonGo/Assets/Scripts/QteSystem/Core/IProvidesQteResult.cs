using System;

namespace QteSystem.Core
{
    public interface IProvidesQteResult
    {
        event Action<QteResult> OnReached;
    }
}