using System;
using UI.BaseUI.Presenters;

namespace UI.DodgeUI
{
	public interface IDodgeView : IView
	{
		event Action Dodged;
		void Destroy();
	}
}