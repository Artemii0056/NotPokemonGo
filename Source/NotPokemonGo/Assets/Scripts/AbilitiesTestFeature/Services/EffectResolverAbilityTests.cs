using System;
using Effects;
using Stats;
using Units;

namespace AbilitiesTestFeature.Services
{ 
	public class EffectResolverAbilityTests : IEffectResolver
	{
		public event Action<EffectResolver.EffectDataPayload> EffectApplied;


		public void ApplyEffect(Unit source, Unit target, EffectInfo effect) 
		{
			if (target.GetStat(StatType.Invulnerability) != 0)
				return;
			
			float finalValue = CalculateStatModification(source,target, effect.TargetType, effect.Type, effect.Value); 
			EffectApplied?.Invoke(new EffectResolver.EffectDataPayload(source, target, finalValue, effect));
			target.ChangeStatValue(finalValue, effect.TargetType);
		}
		
		private float CalculateStatModification(
			Unit source,
			Unit target,
			StatType targetStat,
			EffectType effectType,
			float baseValue)
		{
			float qteModificator = source.GetStat(StatType.QteDamageModifier);
			
			float finalValue;

			if (qteModificator > 0)
			{
				finalValue = baseValue * (qteModificator + 1);
			}
			else
			{
				finalValue = baseValue;
			}

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


					// if (baseValue < 0)
					// {
					//     // Damage: учитывать броню
					//     float armor = target.GetStat(StatType.ArmorChance);
					//     finalValue = -Math.Min(0, baseValue); //TODO Добавить броню!!!
					//     
					//     Debug.Log(finalValue);
					// }
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