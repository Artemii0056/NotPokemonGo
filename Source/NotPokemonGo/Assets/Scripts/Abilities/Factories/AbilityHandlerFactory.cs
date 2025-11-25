using System;
using Abilities.Bennet;
using Abilities.Enemies;
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
				AbilityType.FrostBall => new BaseEnemyAttack(_coroutineRunner, model),
				AbilityType.EngineeringSeries => new EngineeringSeriesAbility(model, _coroutineRunner, _qteService),
				AbilityType.HittingGround => new HittingGround(_coroutineRunner, model),
				AbilityType.CounterAttack => new Counterattack(_coroutineRunner, model),
				AbilityType.BennetBaseAttack => new BennetBaseAttack(_coroutineRunner, model),
				AbilityType.Generic => new GenericAbilityHandler(_coroutineRunner, model, _abilityStepExecutor),
				_ => throw new ArgumentOutOfRangeException(nameof(model.AbilityType), model.AbilityType, null)
			};
		}
	}
}