using AbilitiesTestFeature.Services;
using AbilitiesTestFeature.UI.Presenters;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.States.Interfaces;
using Services.BattleSessionService;
using Services.BattleUnitContainers;
using Services.UIServices;
using Units;

namespace AbilitiesTestFeature.BattleStates
{
	public class CreateBattlfieldAbilityTestState : IPayloadedState<AbilitiesTestFeatureConfig>
	{
		private readonly IUIService _uiService;
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly ITargetSelector _targetSelector;
		private readonly IBattlefieldFactory _battlefieldFactory;
		private readonly IUnitReadyService _unitReadyService;
		private readonly IBattlefieldProvider _battlefieldProvider;

		public CreateBattlfieldAbilityTestState(
			IBattlefieldFactory battlefieldFactory, 
			IUIService uiService, 
			IBattleStateMachine battleStateMachine,
			ITargetSelector targetSelector, 
			IUnitReadyService unitReadyService,
			IBattlefieldProvider battlefieldProvider)
		{
			_battlefieldFactory = battlefieldFactory;
			_uiService = uiService;
			_battleStateMachine = battleStateMachine;
			_targetSelector = targetSelector;
			_unitReadyService = unitReadyService;
			_battlefieldProvider = battlefieldProvider;
		}
		
		public void Enter(AbilitiesTestFeatureConfig abilitiesTestFeatureConfig)
		{
			_uiService.Show<IAbilityTestPresenter>();

			Battlefield battlefield = _battlefieldFactory.Create(
				abilitiesTestFeatureConfig.HeroConfigs,
				abilitiesTestFeatureConfig.EnemyConfigs);
			
			_battlefieldProvider.Remember(battlefield);
			
			_targetSelector.SetPlatoons(battlefield.EnemyPlatoon, battlefield.HeroesPlatoon);
			_unitReadyService.SetPlatoons(battlefield.HeroesPlatoon.AliveUnits, battlefield.EnemyPlatoon.AliveUnits);
			_battleStateMachine.Enter<PlayerTurnBattleStateAbilityTest, Battlefield>(battlefield);
		}

		public void Exit()
		{
		
		}
	}
}