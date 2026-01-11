using System;
using UI.BaseUI;
using UI.BaseUI.Interfaces;

namespace UI.DodgeUI
{
	public interface IDodgeView : IView
	{
		event Action Dodged;
		void Destroy();
	}
}