using System;
using UI.BaseUI.Interfaces;

namespace AbilitiesTestFeature.UI
{
	public interface IAbilityTestPanel : IView
	{
		event Action<bool> EnableTakeHitChanged;
		event Action EnemyActionButtonClicked;
	}
}