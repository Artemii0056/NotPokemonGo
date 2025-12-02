using System;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DodgeUI
{
	public class DodgeView : MonoBehaviour, IDodgeView
	{
		[SerializeField] private Button _dodgeButton;

		public event Action<Unit> Dodged;

		public void Destroy() => 
			Destroy(gameObject);

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

		private void OnDodgeButtonCLicked()
		{
			Unit unit = FindAnyObjectByType<Unit>();
			Dodged?.Invoke(unit);
		}
	}
}