using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Battlefields;
using Platoons;
using Services.InputServices;
using Services.RaycastServices;
using UI.Ability;
using Units;
using UnityEngine;
using VContainer;

namespace AbilitiesTestFeature.Services
{
	public class FriendActionStrategyAbilityTest
	{
		private readonly Battlefield _battlefield;
		private readonly Unit _source;

		private ISourceProvider _sourceProvider;
		private IAbilityProvider _abilityProvider;
		private AbilityPanelPresenter _abilityPanelPresenter;
		private IInputReader _inputReader;
		private IRaycastService _raycastService;
        
		private IAbilityService _abilityService;

		public FriendActionStrategyAbilityTest(Battlefield battlefield, Unit source)
		{
			_source = source;
			_battlefield = battlefield;
		}

		[Inject]
		public void Initialize(
			IRaycastService raycastService,
			ISourceProvider sourceProvider,
			IAbilityProvider abilityProvider,
			AbilityPanelPresenter abilityPanelPresenter,
			IInputReader inputReader,
			IAbilityService abilityService
		)
		{
			_raycastService = raycastService;
			_inputReader = inputReader;
			_abilityProvider = abilityProvider;
			_sourceProvider = sourceProvider;
			_abilityPanelPresenter = abilityPanelPresenter;
			_abilityService = abilityService;
		}

		public void Enable()
		{
			//ShowAbilityInfos(_source.AbilityModels);
			_sourceProvider.Remember(_source);

			_inputReader.LeftMouseButtonPressed += OnLeftMouseButtonPressed;
		}

		public void Disable()
		{
			_inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonPressed;
		}

		private void OnLeftMouseButtonPressed()
		{
			if (_raycastService.Raycast(out Unit unit)) 
				OnUnitSearched(unit);
		}

		private void OnUnitSearched(Unit unit)
		{
			if (_abilityProvider.AbilityModel == null)
				return;

			if (_source == unit)
				return;

			switch (unit.PlatoonType)
			{
				case PlatoonType.Heroes:
					Debug.Log("Выбрали союзника");
					break;

				case PlatoonType.Enemies: 
					_abilityService.SetBattlefield(_battlefield);
					//_abilityService.Handle(_source, unit, _abilityProvider.AbilityModel);
                    
					_abilityPanelPresenter.Disable();
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void ShowAbilityInfos(List<AbilityModel> abilityModels)
		{
			_abilityPanelPresenter.Enable();
			_abilityPanelPresenter.FillAbilityView(abilityModels);
		}
	}
}