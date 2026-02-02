using AbilitiesTestFeature.BattleStates;
using Infrastructure.StateMachines.BattleStateMachine;
using UnityEngine;
using VContainer;

namespace AbilitiesTestFeature.Initializers
{
	public class SceneAbilityTestInitializer : MonoBehaviour
	{
		[SerializeField] private AbilitiesTestFeatureConfig _abilitiesTestFeatureConfig;

		private IBattleStateMachine _battleStateMachine;

		[Inject]
		private void Construct(IBattleStateMachine battleStateMachine)
		{
			_battleStateMachine = battleStateMachine;
			_battleStateMachine.Enter<CreateBattlfieldAbilityTestState, AbilitiesTestFeatureConfig>(_abilitiesTestFeatureConfig);
		}
	}
}