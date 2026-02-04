using System;
using Effects;
using Pools;
using UnityEngine;

namespace Services.EffectViewServices
{
    public sealed class CombatTextPresenter : IDisposable
    {
        private readonly IEffectResolver _resolver;
        private readonly CombatTextPool _pool;
        private readonly RectTransform _popupsRoot;
        private readonly Camera _camera;

        private readonly Vector3 _worldOffset = new(0f, 2.0f, 0f);

        public CombatTextPresenter(
            IEffectResolver resolver,
            CombatTextPool pool,
            RectTransform popupsRoot,
            Camera camera)
        {
            _resolver = resolver;
            _pool = pool;
            _popupsRoot = popupsRoot;
            _camera = camera;

            _resolver.EffectApplied += OnEffectApplied;

            Debug.Log("[CombatTextPresenter] Started");
        }

        public void Dispose() => 
            _resolver.EffectApplied -= OnEffectApplied;

        private void OnEffectApplied(EffectResolver.EffectDataPayload payload)
        {
            if (payload.Target == null) 
                return;
        
            if (Math.Abs(payload.FinalValue) < Mathf.Epsilon) 
                return;

            CombatText view = _pool.Get(_popupsRoot);

            Vector3 worldPos = payload.Target.transform.position + _worldOffset;
            Vector3 screenPos = _camera.WorldToScreenPoint(worldPos);

            RectTransform root = _popupsRoot;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                root, screenPos, null, out Vector2 localPoint);

            view.RectTransform.anchoredPosition = localPoint;

            view.Play(payload.FinalValue, payload.Effect.Type, () => _pool.Return(view));
        }
    }
}