using System;
using UI.BaseUI.Presenters;
using Units;

namespace UI.DodgeUI
{
	public interface IDodgePresenter : IPresenter
	{
		event Action<Unit> Dodged;
	}
}