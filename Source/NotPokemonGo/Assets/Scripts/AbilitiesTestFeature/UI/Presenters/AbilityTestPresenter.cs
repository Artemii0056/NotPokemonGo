using UI.BaseUI.Views;

namespace AbilitiesTestFeature.UI.Presenters
{
	public class AbilityTestPresenter : Presenter<IAbilityTestPanel>, IAbilityTestPresenter
	{
		private IAbilityTestPanel _view;

		public AbilityTestPresenter(IAbilityTestPanel view) : base(view)
		{
			_view = view;
		}

		public override void Activate()
		{
			base.Activate();
		}

		public override void Deactivate()
		{
			base.Deactivate();
		}
	}
}