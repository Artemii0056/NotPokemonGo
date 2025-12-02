using Infrastructure.StateMachines.States.Interfaces;
using UI.DodgeUI;
using UI.Factory;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
	public class PlayerDodgeState : IState
	{
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly IUIFactory _uiFactory;

		private IDodgePresenter _dodgePresenter;

		public PlayerDodgeState(IBattleStateMachine battleStateMachine, IUIFactory uiFactory)
		{
			_battleStateMachine = battleStateMachine;
			_uiFactory = uiFactory;
		}
		
		public void Enter()
		{
			IDodgeView dodgeView = _uiFactory.CreateDodgeView();

			_dodgePresenter = new DodgePresenter(dodgeView);
			_dodgePresenter.Enable();
		}

		public void Exit()
		{
			_dodgePresenter.Disable();
		}
	}
}