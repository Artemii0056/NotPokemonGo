using System;
using UI.BaseUI.Implemenation;
using UnityEngine;
using UnityEngine.UI;

namespace AbilitiesTestFeature.UI
{
	public class AbilityTestPanel : View, IAbilityTestPanel
	{
		[SerializeField] private Toggle _enableTakeHit;
		[SerializeField] private Toggle _enableEnemyStep;
		[SerializeField] private Toggle _enableUnitAgility;

		[SerializeField] private Button _repeatButton;

		public event Action<bool> EnableTakeHitChanged;
		public event Action<bool> EnableEnemyStepChanged;
		public event Action<bool> EnableUnitAgilityChanged;

		public event Action RepeatButtonClicked;

		protected override void OnActivate()
		{
			base.OnActivate();
			_repeatButton.onClick.AddListener(OnRepeatButtonClicked);
			_enableTakeHit.onValueChanged.AddListener(OnEnableTakeHitChanged);
			_enableEnemyStep.onValueChanged.AddListener(OnEnableEnemyStepChanged);
			_enableUnitAgility.onValueChanged.AddListener(OnEnableUnitAgilityChanged);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_repeatButton.onClick.AddListener(OnRepeatButtonClicked);
			_enableTakeHit.onValueChanged.RemoveListener(OnEnableTakeHitChanged);
			_enableEnemyStep.onValueChanged.RemoveListener(OnEnableEnemyStepChanged);
			_enableUnitAgility.onValueChanged.RemoveListener(OnEnableUnitAgilityChanged);
		}

		private void OnRepeatButtonClicked() => 
			RepeatButtonClicked?.Invoke();

		private void OnEnableUnitAgilityChanged(bool value) =>
			EnableUnitAgilityChanged?.Invoke(value);

		private void OnEnableEnemyStepChanged(bool value) =>
			EnableEnemyStepChanged?.Invoke(value);

		private void OnEnableTakeHitChanged(bool value) =>
			EnableTakeHitChanged?.Invoke(value);
	}
}