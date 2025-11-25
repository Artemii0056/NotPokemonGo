using System;
using Abilities.AbilityTypes;
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

		public AbilityHandlerFactory(ICoroutineRunner coroutineRunner, IQteService qteService)
		{
			_coroutineRunner = coroutineRunner;
			_qteService = qteService;
		}

		public IAbilityHandler Create(AbilityModel model)
		{
			return model.AbilityType switch
			{
				AbilityType.FrostBall => new BaseEnemyAttack(_coroutineRunner, model),
				AbilityType.EngineeringSeries => new EngineeringSeriesAbility(model, _coroutineRunner, _qteService),
				AbilityType.HittingGround => new HittingGround(_coroutineRunner, model),
				AbilityType.BaseAttack => new AbilityBaseAttack(model, _coroutineRunner),
				AbilityType.CounterAttack => new Counterattack(_coroutineRunner, model),
				_ => throw new ArgumentOutOfRangeException(nameof(model.AbilityType), model.AbilityType, null)
			};
		}
	}
}