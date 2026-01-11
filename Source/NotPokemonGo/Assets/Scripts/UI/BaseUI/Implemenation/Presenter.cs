using UI.BaseUI.Interfaces;

namespace UI.BaseUI.Views
{
	public class Presenter <T> : IPresenter where T : IView
	{
		protected readonly T View;

		public Presenter(T view) => 
			View = view;

		public virtual void Activate() => 
			View.Activate();

		public virtual void Deactivate() => 
			View.Deactivate();

		public virtual void Dispose()
		{ }
	}
}