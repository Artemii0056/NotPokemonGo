using System;

namespace Armaments.Movers
{
    /// <summary>
    /// Optional interface for movers that support ability-run ownership.
    /// If scopeId is set, all DOTween tweens created by the mover must be tagged with it.
    /// </summary>
    public interface IAbilityScopeOwnedMover
    {
        void SetScopeId(int scopeId);
    }
}
