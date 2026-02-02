using AbilitiesTestFeature.Services;
using AbilitiesTestFeature.UI.Presenters;
using AbilitiesTestFeature.UI.Views;
using Infrastructure.StateMachines.BattleStateMachine;
using Services.UIServices;
using UnityEngine;
using VContainer;

namespace AbilitiesTestFeature.Initializers
{
	public class UIAbilityTestInitializer : MonoBehaviour
	{
		[SerializeField] private AbilityTestPanel _abilityTestPanel;
	
		private IPresenterRegistrar _presenterRegistrar;

		[Inject]
		private void Construct(
			IPresenterRegistrar presenterRegistrar, 
			IBattlefieldProvider battlefieldProvider,
			IBattleStateMachine battleStateMachine)
		{
			_presenterRegistrar = presenterRegistrar;
			_presenterRegistrar.RegisterPresenter<IAbilityTestPresenter>(
				new AbilityTestPresenter(
					_abilityTestPanel,
					battlefieldProvider,
					battleStateMachine));
		}

		private void OnDestroy()
		{
			_presenterRegistrar.UnregisterPresenter<IAbilityTestPresenter>();
		}
	}
}