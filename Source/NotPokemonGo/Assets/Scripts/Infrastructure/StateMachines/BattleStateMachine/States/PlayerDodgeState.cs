using System.Collections;
using System.Collections.Generic;
using Characters.Configs;
using DodgeSystem.Configs;
using Infrastructure.StateMachines.States.Interfaces;
using Services;
using Services.InputServices;
using Services.RaycastServices;
using Services.StaticDataServices;
using Stats;
using UI.DodgeUI;
using UI.Factory;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
	public class PlayerDodgeState : IState //TODO DELETE
	{
		private readonly IBattleStateMachine _battleStateMachine;
		private readonly IStaticDataService _staticDataService;
		private readonly IInputReader _inputReader;
		private readonly IRaycastService _raycastService;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly IUIFactory _uiFactory;
		
		private readonly List<UnitType> _units = new List<UnitType>(4);

		private IDodgePresenter _dodgePresenter;
		private IDodgeView _dodgeView;

		public PlayerDodgeState(
			IBattleStateMachine battleStateMachine,
			IUIFactory uiFactory,
			IStaticDataService staticDataService,
			IInputReader inputReader,
			IRaycastService raycastService,
			ICoroutineRunner coroutineRunner)
		{
			_battleStateMachine = battleStateMachine;
			_uiFactory = uiFactory;
			_staticDataService = staticDataService;
			_inputReader = inputReader;
			_raycastService = raycastService;
			_coroutineRunner = coroutineRunner;
		}

		public void Enter()
		{
			_dodgeView = _uiFactory.CreateDodgeView();
			_dodgeView.Hide();
			
			_dodgePresenter = new DodgePresenter(_dodgeView, _inputReader, _raycastService);
			_dodgePresenter.Enable();
			_dodgePresenter.Dodged += OnDodged;
		}

		public void Exit()
		{
			_units.Clear();

			_dodgePresenter.Dodged -= OnDodged;
			_dodgePresenter.Disable();
			_dodgePresenter = null;
			_dodgeView.Destroy();
		}

		private void OnDodged(Unit unit)
		{
			if (_units.Contains(unit.UnitType) == false)
			{
				DodgeConfig dodgeConfig = _staticDataService.GetDodgeConfigByUnitType(unit.UnitType);

				unit.ChangeStatValue(1, StatType.DodgeFlag);

				unit.UnitAnimatorController.Play(dodgeConfig.AnimationCashName);
				
				_units.Add(unit.UnitType);
				_coroutineRunner.StartCoroutine(Dodge(dodgeConfig.Duration, unit));
			}
		}

		private IEnumerator Dodge(float duration, Unit unit)
		{
			yield return new WaitForSeconds(duration);
			
			unit.ChangeStatValue(0, StatType.DodgeFlag);
			_units.Remove(unit.UnitType);
		}
	}
}