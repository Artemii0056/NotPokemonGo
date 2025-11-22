using System;
using Abilities.Bennet;
using Abilities.MV;
using Services;
using Units;

namespace Abilities.AbilityTypes
{
	public abstract class AbilityBaseType : IAbilityHandler
	{
		public AbilityModel AbilityModel { get; }
		public ICoroutineRunner CoroutineRunner { get; }

		protected AbilityBaseType(AbilityModel abilityModel, ICoroutineRunner coroutineRunner)
		{
			AbilityModel = abilityModel;
			CoroutineRunner = coroutineRunner;
		}

		public abstract event Action<IAbilityHandler> Finished;
		public abstract void Play(Unit source, Unit target);

		public abstract void Stop();
	}
}