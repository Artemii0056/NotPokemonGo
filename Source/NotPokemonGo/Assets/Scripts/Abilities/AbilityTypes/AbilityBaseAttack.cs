using System;
using Abilities.Bennet;
using Abilities.MV;
using Services;
using Units;

namespace Abilities.AbilityTypes
{
	public class AbilityBaseAttack : AbilityBaseType
	{
		public AbilityBaseAttack(AbilityModel abilityModel, ICoroutineRunner coroutineRunner) : base(abilityModel, coroutineRunner)
		{
		}

		public override event Action<IAbilityHandler> Finished;
		public override void Play(Unit source, Unit target)
		{
			throw new NotImplementedException();
		}

		public override void Stop()
		{
			throw new NotImplementedException();
		}
	}
}