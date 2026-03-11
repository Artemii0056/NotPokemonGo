using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public sealed class CombatTextPool : ICombatTextPool
    {
        private readonly CombatText.CombatText _prefab;
        private readonly Stack<CombatText.CombatText> _stack = new();
        private readonly Transform _stashRoot;

        public CombatTextPool(CombatText.CombatText prefab, int prewarm = 10)
        {
            _prefab = prefab;

            var go = new GameObject("[Pool] CombatText");
            
            go.SetActive(false);
            _stashRoot = go.transform;

            for (int i = 0; i < prewarm; i++)
            {
                var v = Object.Instantiate(_prefab, _stashRoot, false);
                v.gameObject.SetActive(false);
                _stack.Push(v);
            }
        }

        public CombatText.CombatText Get(RectTransform parent)
        {
            while (_stack.Count > 0)
            {
                var v = _stack.Pop();
                
                if (v != null) 
                {
                    v.transform.SetParent(parent, false);
                    v.gameObject.SetActive(true);
                    return v;
                }
            }

            var created = Object.Instantiate(_prefab, parent, false);
            created.gameObject.SetActive(true);
            return created;
        }


        public void Return(CombatText.CombatText view)
        {
            if (view == null) 
                return;
            
            view.gameObject.SetActive(false);
            view.transform.SetParent(_stashRoot, false);
            _stack.Push(view);
        }
    }
}