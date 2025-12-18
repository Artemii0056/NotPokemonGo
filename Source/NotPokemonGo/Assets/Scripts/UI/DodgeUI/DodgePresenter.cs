using System;
using Platoons;
using Services.InputServices;
using Services.RaycastServices;
using Units;
using UnityEngine;

namespace UI.DodgeUI
{
	public class DodgePresenter : IDodgePresenter
	{
		private readonly IDodgeView _dodgeView;
		private readonly IInputReader _inputReader;
		private readonly IRaycastService _raycastService;
		
		private Unit _unit;

		public event Action<Unit> Dodged;

		public DodgePresenter
		(
			IDodgeView dodgeView, 
			IInputReader inputReader, 
			IRaycastService raycastService
		)
		{
			_dodgeView = dodgeView;
			_inputReader = inputReader;
			_raycastService = raycastService;
		}

		public void Enable()
		{
			_inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
			_dodgeView.Dodged += OnDodged;
		}

		public void Disable()
		{
			_inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
			_dodgeView.Dodged -= OnDodged;
		}

		private void OnLeftMouseButtonPressed()
		{
			if (_raycastService.Raycast(out Unit unit))
			{
				// if (_unit != null) 
				// 	_dodgeView.Show();

				if (unit.PlatoonType != PlatoonType.Heroes)
					return;
				
				_unit = unit;
			}
			else
			{
				// _dodgeView.Hide();
				// _unit = null;
			}
		}

		private void OnDodged()
		{
			Debug.LogError("OnDodged");
		Dodged?.Invoke(_unit);
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}
	}
}