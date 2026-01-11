using UI.BaseUI.Interfaces;

namespace Services.UIServices
{
	public interface IUIService
	{
		void Show<TPresenter>() where TPresenter : IPresenter;
		void Close<TPresenter>() where TPresenter : IPresenter;
		void CloseAll();
		void Clear();
	}
}