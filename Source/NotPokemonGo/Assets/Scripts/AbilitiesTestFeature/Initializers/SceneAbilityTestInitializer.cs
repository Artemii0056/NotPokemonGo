using AbilitiesTestFeature;
using AbilitiesTestFeature.BattleStates;
using Infrastructure.StateMachines.BattleStateMachine;
using Services.BattleUnitContainers;
using UnityEngine;
using VContainer;

public class SceneAbilityTestInitializer : MonoBehaviour
{
	[SerializeField] private AbilitiesTestFeatureConfig _abilitiesTestFeatureConfig;

	private IBattlefieldFactory _battlefieldFactory;
	private IBattleStateMachine _battleStateMachine;

	[Inject]
	private void Construct(IBattlefieldFactory battlefieldFactory, IBattleStateMachine battleStateMachine)
	{
		_battleStateMachine = battleStateMachine;
		_battlefieldFactory = battlefieldFactory;

		_battlefieldFactory.Create(
			_abilitiesTestFeatureConfig.HeroConfigs,
			_abilitiesTestFeatureConfig.EnemyConfigs);
		
		_battleStateMachine.Enter<BattleStateAbilityTest>();
	}
}