using Armaments;
using DodgeSystem;
using DodgeSystem.Configs;
using Effects;
using Infrastructure.StateMachines.States.Interfaces;
using Services.StaticDataServices;
using Stats;
using Statuses;
using Statuses.Services;
using UI.DodgeUI;
using UI.Factory;
using Units;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
	public class PlayerDodgeState : IState
	{
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly IEffectResolver _effectResolver;
		private readonly IStaticDataService _staticDataService;
		private readonly IStatusResolver _statusResolver;
		private readonly IUIFactory _uiFactory;

		private IDodgePresenter _dodgePresenter;
		private IArmamentMover _armamentMover;
		private IDodgeView _dodgeView;

		public PlayerDodgeState(
			IBattleStateMachine battleStateMachine, 
			IUIFactory uiFactory, 
			IEffectResolver effectResolver,
			IStaticDataService staticDataService,
			IStatusResolver statusResolver
			)
		{
			_battleStateMachine = battleStateMachine;
			_uiFactory = uiFactory;
			_effectResolver = effectResolver;
			_staticDataService = staticDataService;
			_statusResolver = statusResolver;
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
			DodgeConfig dodgeConfig = _staticDataService.GetDodgeConfigByUnitType(unit.UnitType);
			Status dodge = new DodgeStatus(); 
			_statusResolver.Resolve(dodge, unit);
			_effectResolver.ApplyEffect(unit, new EffectInfo(1, StatType.DodgeFlag));
		}
	}
}