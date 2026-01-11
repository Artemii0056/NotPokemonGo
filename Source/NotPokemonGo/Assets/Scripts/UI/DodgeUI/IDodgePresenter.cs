using System;
using UI.BaseUI.Interfaces;
using Units;

namespace UI.DodgeUI
{
	public interface IDodgePresenter : IPresenter
	{
		event Action<Unit> Dodged;
	}
}