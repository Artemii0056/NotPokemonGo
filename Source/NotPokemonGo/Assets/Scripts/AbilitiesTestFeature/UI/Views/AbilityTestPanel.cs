using System;
using UI.BaseUI.Implemenation;
using UnityEngine;
using UnityEngine.UI;

namespace AbilitiesTestFeature.UI.Views
{
	public class AbilityTestPanel : View, IAbilityTestPanel
	{
		[SerializeField] private Toggle _enableTakeHit;

		[SerializeField] private Button _enemyAction;

		public event Action<bool> EnableTakeHitChanged;

		public event Action EnemyActionButtonClicked;

		protected override void OnActivate()
		{
			base.OnActivate();
			_enemyAction.onClick.AddListener(OnEnemyActionButtonClicked);
			_enableTakeHit.onValueChanged.AddListener(OnEnableTakeHitChanged);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_enemyAction.onClick.AddListener(OnEnemyActionButtonClicked);
			_enableTakeHit.onValueChanged.RemoveListener(OnEnableTakeHitChanged);
		}

		private void OnEnemyActionButtonClicked() => 
			EnemyActionButtonClicked?.Invoke();

		private void OnEnableTakeHitChanged(bool value) =>
			EnableTakeHitChanged?.Invoke(value);
	}
}