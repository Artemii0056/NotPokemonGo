using Armaments;
using DodgeSystem;
using Effects;
using Infrastructure.StateMachines.States.Interfaces;
using Stats;
using UI.DodgeUI;
using UI.Factory;
using Units;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
	public class PlayerDodgeState : IState
	{
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly IArmamentApplicatorService _armamentApplicatorService;
		private readonly IDodgeService _dodgeService;
		private readonly IEffectResolver _effectResolver;
		private readonly IUIFactory _uiFactory;

		private IDodgePresenter _dodgePresenter;
		private IArmamentMover _armamentMover;
		private IDodgeView _dodgeView;

		public PlayerDodgeState(
			IBattleStateMachine battleStateMachine, 
			IUIFactory uiFactory, 
			IArmamentApplicatorService armamentApplicatorService,
			IDodgeService dodgeService,
			IEffectResolver effectResolver
			)
		{
			_battleStateMachine = battleStateMachine;
			_uiFactory = uiFactory;
			_armamentApplicatorService = armamentApplicatorService;
			_dodgeService = dodgeService;
			_effectResolver = effectResolver;
		}
		
		public void Enter()
		{
			_dodgeView = _uiFactory.CreateDodgeView();

			_dodgePresenter = new DodgePresenter(_dodgeView);
			_dodgePresenter.Enable();
			_dodgePresenter.Dodged += OnDodged;
		}

		public void Exit()
		{
			_dodgePresenter.Dodged -= OnDodged;
			_dodgePresenter.Disable();
			_dodgeView.Destroy();
		}

		private void OnDodged(Unit unit)
		{
			_effectResolver.ApplyEffect(unit, new EffectInfo(1, StatType.DodgeFlag));
		}
	}
}