using System;
using Effects;
using Stats;
using Units;

namespace AbilitiesTestFeature.Services
{ 
	public class EffectResolverAbilityTests : IEffectResolver
	{
		public event Action<EffectDataPayload> EffectApplied;


		public void ApplyEffect(Unit target, EffectInfo effect) 
		{
			if (target.GetStat(StatType.Invulnerability) != 0)
				return;
			
			float finalValue = CalculateStatModification(target, effect.TargetType, effect.Type); 
			EffectApplied?.Invoke(new EffectDataPayload(target, finalValue, effect));
			target.ChangeStatValue(finalValue, effect.TargetType);
		}
		
		private float CalculateStatModification(
			Unit target,
			StatType targetStat,
			EffectType effectType)
		{
			float finalValue = 0;

			switch (targetStat)
			{
				case StatType.Health:
					switch (effectType)
					{
						case EffectType.Damage:
							if (target.IsAlive)
							{
								finalValue = -finalValue;
								// target.UnitAnimatorController.Play(Constants.BaseAnimations.TakeDamage);
							}
							else
							{
								finalValue = 0;
							}

							// Debug.Log(finalValue);
							break;

						case EffectType.Heal:
							break;

						default:
							throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
					}
					break;

				case StatType.AgilityRestoreSpeed:
					break;

				case StatType.ArmorChance:
					break;

				case StatType.Mana:
					break;
			}

			return finalValue;
		}
	}
}