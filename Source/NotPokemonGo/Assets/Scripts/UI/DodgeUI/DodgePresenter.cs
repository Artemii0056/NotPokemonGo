namespace UI.DodgeUI
{
	public class DodgePresenter : IDodgePresenter
	{
		private readonly IDodgeView _dodgeView;

		public DodgePresenter(IDodgeView dodgeView)
		{
			_dodgeView = dodgeView;
		}
		
		public void Enable() => 
			_dodgeView.Dodged += OnDodged;

		public void Disable() => 
			_dodgeView.Dodged -= OnDodged;

		private void OnDodged()
		{
		}
	}
}