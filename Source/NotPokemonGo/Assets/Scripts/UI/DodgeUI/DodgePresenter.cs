using System;
using Abilities;
using DodgeSystem;
using Infrastructure.StateMachines.BattleStateMachine;
using Units;

namespace UI.DodgeUI
{
	public class DodgePresenter : IDodgePresenter
	{
		private readonly IDodgeView _dodgeView;

		public event Action<Unit> Dodged;

		public DodgePresenter(IDodgeView dodgeView) => 
			_dodgeView = dodgeView;

		public void Enable() => 
			_dodgeView.Dodged += OnDodged;

		public void Disable() => 
			_dodgeView.Dodged -= OnDodged;

		private void OnDodged(Unit unit) => 
			Dodged?.Invoke(unit);
	}
}