using UnityEngine;

namespace Pools
{
    public interface ICombatTextPool
    {
        CombatText Get(RectTransform parent);
        void Return(CombatText view);
    }
}