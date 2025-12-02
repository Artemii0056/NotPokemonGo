using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DodgeUI
{
	public class DodgeView : MonoBehaviour, IDodgeView
	{
		[SerializeField] private Button _dodgeButton;

		public event Action Dodged;

		public void Show()
		{
			gameObject.SetActive(true);
			_dodgeButton.onClick.AddListener(OnDodgeButtonCLicked);
		}

		public void Hide()
		{
			_dodgeButton.onClick.RemoveListener(OnDodgeButtonCLicked);
			gameObject.SetActive(false);
		}

		private void OnDodgeButtonCLicked() => 
			Dodged?.Invoke();
	}
}