using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public sealed class CombatTextPool : MonoBehaviour
    {
        private readonly CombatText _prefab;
        private readonly Stack<CombatText> _stack = new();

        public CombatTextPool(CombatText prefab, int prewarm = 10)
        {
            _prefab = prefab;
            
            for (int i = 0; i < prewarm; i++)
                _stack.Push(Object.Instantiate(_prefab));
        }

        public CombatText Get(RectTransform parent)
        {
            CombatText combatText = _stack.Count > 0 ? _stack.Pop() : Object.Instantiate(_prefab);
            Debug.Log($"[CombatTextPool] Rent: active={combatText.gameObject.activeSelf}, parent={parent.name}");

            combatText.transform.SetParent(parent, false);
            
            Debug.Log($"[CombatTextPool] After parent: {combatText.transform.parent?.name}, scale={combatText.RectTransform.localScale}");

            combatText.gameObject.SetActive(true);
            
            return combatText;
        }

        public void Return(CombatText view)
        {
            view.gameObject.SetActive(false);
            view.transform.SetParent(null, false);
            _stack.Push(view);
        }
    }
}