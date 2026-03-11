using UnityEngine;

namespace Pools
{
    public interface ICombatTextPool
    {
        CombatText.CombatText Get(RectTransform parent);
        void Return(CombatText.CombatText view);
    }
}