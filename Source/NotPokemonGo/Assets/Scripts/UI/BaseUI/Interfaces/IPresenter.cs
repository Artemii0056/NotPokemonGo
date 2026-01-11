using System;

namespace UI.BaseUI.Interfaces
{
	public interface IPresenter : IDisposable
	{
		public void Activate();
		public void Deactivate();
	}
}