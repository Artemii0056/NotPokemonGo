using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Battlefields;
using Units;
using UnityEngine;
using VContainer;

namespace AbilitiesTestFeature.Services
{
	public class EnemyUnitActionStrategyAbilityTest
	{
		private readonly Battlefield _battlefield;
		private readonly Unit _source;

		private ISourceProvider _sourceProvider;
		private IAbilityService _abilityService;

		public EnemyUnitActionStrategyAbilityTest(Battlefield battlefield, Unit source)
		{
			_source = source;
			_battlefield = battlefield;
		}

		[Inject]
		public void Initialize(
			ISourceProvider sourceProvider,
			IAbilityService abilityService
		)
		{
			_sourceProvider = sourceProvider;
			_abilityService = abilityService;
		}

		public void Enable()
		{
			Attack(_battlefield.HeroesPlatoon.AliveUnits);
		}

		public void Disable()
		{
			_sourceProvider.Discard(); //Todo Сбрасываться должен в стейтмашине 
		}

		private void
			Attack(List<Unit> targets) //Имеем несколько проблем. 1 - именно тут рандомится враг, хотя абилка может наносить атаку по разным таргетам.
			//2 - Изменяет стейт. Этого тут не должно происходить. Только если "включить стейт ходьбы врага"
			//3 - работает с абилити сервисом. Нужно эту логику разбить. Враг просто ходит - выбирает способность, которую применит и все, дальше уже логика способности
			//4 -  
		{
			AbilityModel randomAbility = _source.AbilityModels[Random.Range(0, _source.AbilityModels.Count)];

			Unit randomTarget = GetRandomTarget(targets);

			_abilityService.SetBattlefield(_battlefield);
			_abilityService.Handle(_source, randomTarget, randomAbility);
		}

		private Unit GetRandomTarget(List<Unit> targets) =>
			targets[Random.Range(0, targets.Count)];
	}
}