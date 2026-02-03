using System;
using Effects;
using Units;
using UnityEngine;

namespace Services.EffectViewServices
{
	public class EffectViewService : IDisposable
	{
		private readonly IEffectResolver _resolver;
		private readonly UnitViewRegistry _registry;

		public EffectViewService(IEffectResolver resolver, UnitViewRegistry registry)
		{
			_resolver = resolver;
			_registry = registry;
			_resolver.EffectApplied += OnEffectApplied;
		}

		public void Dispose() => 
			_resolver.EffectApplied -= OnEffectApplied;

		private void OnEffectApplied(EffectResolver.EffectDataPayload effectDataPayload)
		{
			if (effectDataPayload.Target == null) 
				return;

			UnitDamageView view = _registry.Get(effectDataPayload.Target);

			if (Math.Abs(effectDataPayload.FinalValue) < Mathf.Epsilon) 
				return;

			view.Play(effectDataPayload.FinalValue, effectDataPayload.Effect);
		}
	}
}