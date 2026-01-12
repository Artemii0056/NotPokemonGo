using System.Collections.Generic;
using AbilitiesTestFeature.Services;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using Units;
using UnityEngine;
using VContainer;

namespace AbilitiesTestFeature.BattleStates
{
	public class EnemyTurnBattleStateAbilityTest : IPayloadedState<Battlefield>
	{
		private readonly IObjectResolver _objectResolver;
		private readonly IBattleStateMachine _battleStateMachine;
		private EnemyUnitActionStrategyAbilityTest _enemyUnitActionStrategyAbilityTest;

		public EnemyTurnBattleStateAbilityTest(IObjectResolver objectResolver, IBattleStateMachine  battleStateMachine)
		{
			_objectResolver = objectResolver;
			_battleStateMachine = battleStateMachine;
		}

		public void Enter(Battlefield battlefield)
		{
			Unit randomUnit = GetRandomUnit(battlefield);

			_enemyUnitActionStrategyAbilityTest = new EnemyUnitActionStrategyAbilityTest(battlefield, randomUnit);
			
			_objectResolver.Inject(_enemyUnitActionStrategyAbilityTest);
			_enemyUnitActionStrategyAbilityTest.Enable();
			
			_battleStateMachine.Enter<PlayerTurnBattleStateAbilityTest, Battlefield>(battlefield);
		}

		public void Exit()
		{
			_enemyUnitActionStrategyAbilityTest.Disable();
		}

		private static Unit GetRandomUnit(Battlefield battlefield)
		{
			List<Unit> units = battlefield.EnemyPlatoon.AliveUnits;
			
			return units[Random.Range(0, units.Count)];
		}
	}
}