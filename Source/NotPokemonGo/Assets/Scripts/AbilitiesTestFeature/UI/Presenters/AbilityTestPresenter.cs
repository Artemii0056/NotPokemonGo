using System;
using System.Collections.Generic;
using AbilitiesTestFeature.BattleStates;
using AbilitiesTestFeature.Services;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine;
using Stats;
using UI.BaseUI.Views;
using Units;

namespace AbilitiesTestFeature.UI.Presenters
{
	public class AbilityTestPresenter : Presenter<IAbilityTestPanel>, IAbilityTestPresenter
	{
		private readonly IAbilityTestPanel _view;
		private readonly IBattlefieldProvider _battlefieldProvider;
		private readonly IBattleStateMachine _battleStateMachine;

		public AbilityTestPresenter(
			IAbilityTestPanel view, 
			IBattlefieldProvider battlefieldProvider,
			IBattleStateMachine battleStateMachine) : base(view)
		{
			_view = view;
			_battlefieldProvider = battlefieldProvider;
			_battleStateMachine = battleStateMachine;
		}

		public override void Activate()
		{
			base.Activate();
			_view.EnableTakeHitChanged += OnEnableTakeHitChanged;
			_view.EnemyActionButtonClicked += OnEnemyActionButtonClicked;
		}

		public override void Deactivate()
		{
			base.Deactivate();
			_view.EnableTakeHitChanged -= OnEnableTakeHitChanged;
			_view.EnemyActionButtonClicked -= OnEnemyActionButtonClicked;
		}

		private void OnEnemyActionButtonClicked() => 
			_battleStateMachine.Enter<EnemyTurnBattleStateAbilityTest, Battlefield>(_battlefieldProvider.Get);

		private void OnEnableTakeHitChanged(bool isEnabled)
		{
			if (isEnabled)
			{
				Battlefield battlefield = _battlefieldProvider.Get;
				List<Unit> enemyUnits = battlefield.EnemyPlatoon.AliveUnits;

				SetInvulnerabilityUnits(enemyUnits, isEnabled);
				
				List<Unit> heroesUnits = battlefield.HeroesPlatoon.AliveUnits;

				SetInvulnerabilityUnits(heroesUnits, isEnabled);
			}
			else
			{
				Battlefield battlefield = _battlefieldProvider.Get;
				List<Unit> enemyUnits = battlefield.EnemyPlatoon.AliveUnits;

				SetInvulnerabilityUnits(enemyUnits, isEnabled);
				
				List<Unit> heroesUnits = battlefield.HeroesPlatoon.AliveUnits;

				SetInvulnerabilityUnits(heroesUnits, isEnabled);
			}
		}

		private void SetInvulnerabilityUnits(List<Unit> units, bool isInvulnerability)
		{
			foreach (Unit unit in units) 
				unit.ChangeStatValue(Convert.ToInt32(isInvulnerability), StatType.Invulnerability);
		}
	}
}