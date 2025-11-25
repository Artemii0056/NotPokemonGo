using System;
using Abilities.Bennet;
using Abilities.MV;
using Services;
using Services.QTEServices;

namespace Abilities.Factories
{
	public class AbilityHandlerFactory : IAbilityHandlerFactory
	{
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IQteService _qteService;
		private readonly IAbilityStepExecutor _abilityStepExecutor;

		public AbilityHandlerFactory(
			ICoroutineRunner coroutineRunner, 
			IQteService qteService, 
			IAbilityStepExecutor abilityStepExecutor)
		{
			_coroutineRunner = coroutineRunner;
			_qteService = qteService;
			_abilityStepExecutor = abilityStepExecutor;
		}

		public IAbilityHandler Create(AbilityModel model)
		{
			return model.AbilityType switch
			{
				// AbilityType.EngineeringSeries => new EngineeringSeriesAbility(model, _coroutineRunner, _qteService),
				// AbilityType.CounterAttack => new Counterattack(_coroutineRunner, model),
				AbilityType.Generic => new GenericAbilityHandler(_coroutineRunner, model, _abilityStepExecutor),
				_ => throw new ArgumentOutOfRangeException(nameof(model.AbilityType), model.AbilityType, null)
			};
		}
	}
}