using System;
using UI.BaseUI.Interfaces;

namespace AbilitiesTestFeature.UI
{
	public interface IAbilityTestPanel : IView
	{
		event Action<bool> EnableTakeHitChanged;
		event Action<bool> EnableEnemyStepChanged;
		event Action<bool> EnableUnitAgilityChanged;
		event Action RepeatButtonClicked;
	}
}