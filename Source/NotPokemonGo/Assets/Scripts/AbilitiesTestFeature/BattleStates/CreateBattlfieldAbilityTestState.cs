using AbilitiesTestFeature.UI.Presenters;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using Services.BattleUnitContainers;
using Services.UIServices;
using Units;
using UnityEngine;

namespace AbilitiesTestFeature.BattleStates
{
	public class CreateBattlfieldAbilityTestState : IPayloadedState<AbilitiesTestFeatureConfig>
	{
		private readonly IUIService _uiService;
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly ITargetSelector _targetSelector;
		private readonly IBattlefieldFactory _battlefieldFactory;

		public CreateBattlfieldAbilityTestState(
			IBattlefieldFactory battlefieldFactory, 
			IUIService uiService, 
			IBattleStateMachine battleStateMachine,
			ITargetSelector targetSelector)
		{
			_battlefieldFactory = battlefieldFactory;
			_uiService = uiService;
			_battleStateMachine = battleStateMachine;
			_targetSelector = targetSelector;
		}
		
		public void Enter(AbilitiesTestFeatureConfig abilitiesTestFeatureConfig)
		{
			_uiService.Show<IAbilityTestPresenter>();

			Battlefield battlefield = _battlefieldFactory.Create(
				abilitiesTestFeatureConfig.HeroConfigs,
				abilitiesTestFeatureConfig.EnemyConfigs);
			
			_targetSelector.SetPlatoons(battlefield.EnemyPlatoon, battlefield.HeroesPlatoon);
			_battleStateMachine.Enter<BattleState>();
		}

		public void Exit()
		{
		
		}
	}
}