using AbilitiesTestFeature.UI;
using AbilitiesTestFeature.UI.Presenters;
using Services.UIServices;
using UnityEngine;
using VContainer;

public class UIAbilityTestInitializer : MonoBehaviour
{
	[SerializeField] private AbilityTestPanel _abilityTestPanel;
	private IPresenterRegistrar _presenterRegistrar;

	[Inject]
	private void Construct(IPresenterRegistrar presenterRegistrar)
	{
		_presenterRegistrar = presenterRegistrar;
		_presenterRegistrar.RegisterPresenter<IAbilityTestPresenter>(new AbilityTestPresenter(_abilityTestPanel));
	}

	private void OnDestroy()
	{
		_presenterRegistrar.UnregisterPresenter<AbilityTestPresenter>();
	}
}