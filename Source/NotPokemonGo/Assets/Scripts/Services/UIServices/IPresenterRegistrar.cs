using UI.BaseUI.Interfaces;

namespace Services.UIServices
{
	public interface IPresenterRegistrar
	{
		void RegisterPresenter<T>(T presenter) where T : IPresenter;
		void UnregisterPresenter<T>() where T : IPresenter;
	}
}