using System.Collections.Generic;
using System.Linq;
using DodgeSystem;
using Effects;
using Statuses;
using Statuses.Services;
using Units;

namespace Armaments
{
	public class ArmamentApplicator : IArmamentApplicator
	{
		private readonly IArmamentViewFactory _armamentViewFactory;
		private readonly IStatusFactory _statusFactory;
		private readonly IEffectResolver _effectResolver;
		private readonly IStatusResolver _statusResolver;
		private readonly IArmamentMover _armamentMover;
		private readonly IDodgeService _dodgeService;

		public ArmamentApplicator(
			IArmamentViewFactory armamentViewFactory,
			IStatusFactory statusFactory,
			IEffectResolver effectResolver,
			IStatusResolver statusResolver,
			IArmamentMover armamentMover,
			IDodgeService dodgeService)
		{
			_armamentViewFactory = armamentViewFactory;
			_statusFactory = statusFactory;
			_effectResolver = effectResolver;
			_statusResolver = statusResolver;
			_armamentMover = armamentMover;
			_dodgeService = dodgeService;
		}
        
		public void Apply(ArmamentSetup setup, Unit source, params Unit[] targets)
		{
			foreach (var target in targets)
			{
				List<EffectInfo> effects = CreateEffects(setup.EffectsSetup);
				List<Status> statuses = CreateStatuses(setup.Statuses, source, target);
				
				Armament armament =
					_armamentViewFactory.Create(
						effects,
						statuses,
						source.abilityPos.position,
						setup.ArmamentPrefab,
						source,
						target);
				
				_armamentMover.Move(armament);
				_armamentMover.Reached += OnReached;
			}
		}

		public void Apply(Armament armament)
		{
			_armamentMover.Move(armament);
			_armamentMover.Reached += OnReached;
		}

		private void OnReached(Armament armament)
		{
			_armamentMover.Reached -= OnReached; // непраивльно или правильно

			if (_dodgeService.CanDodge(armament.Target))
				Apply(_dodgeService.Dodge(armament));
			else
				ApplyEffectsOnTarget(armament.Source, armament.Target, armament.Statuses, armament.Effects);
		}

		private List<EffectInfo> CreateEffects(List<EffectSetup> effects) =>
			effects.Select(s => new EffectInfo(s.Value, s.TargetType, s.Type, s.DamageType)).ToList();

		private List<Status> CreateStatuses(IEnumerable<StatusSetup> setups, Unit source, Unit target) =>
			setups.Select(s => _statusFactory.Create(s, source, target, _effectResolver)).ToList();

		private void ApplyEffectsOnTarget(Unit source, Unit target, List<Status> statuses, List<EffectInfo> effects)
		{
			foreach (var status in statuses)
				_statusResolver.Resolve(status, target);

			foreach (var effectInfo in effects)
				_effectResolver.ApplyEffect(source, target, effectInfo);
		}
	}
}