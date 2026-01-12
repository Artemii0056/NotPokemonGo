using AbilitiesTestFeature.Services;
using Battlefields;
using Infrastructure.StateMachines.States.Interfaces;
using Platoons;
using Services.InputServices;
using Services.RaycastServices;
using Services.UIServices;
using UI.Ability;
using Units;
using VContainer;

namespace AbilitiesTestFeature.BattleStates
{
	public class PlayerTurnBattleStateAbilityTest : IPayloadedState<Battlefield>
	{
		private readonly IInputReader _inputReader;
		private readonly IRaycastService _raycastService;
		private readonly IObjectResolver _objectResolver;
		private readonly IUIService _uiService;
		private readonly AbilityPanelPresenter _abilityPanelPresenter;

		private Battlefield _battlefield;
		private FriendActionStrategyAbilityTest _friendUnitActionStrategy;

		public PlayerTurnBattleStateAbilityTest(
			IInputReader inputReader, 
			IRaycastService raycastService,
			IObjectResolver  objectResolver)
		{
			_inputReader = inputReader;
			_raycastService = raycastService;
			_objectResolver = objectResolver;
		}

		public void Enter(Battlefield battlefield)
		{
			_battlefield = battlefield;
			_inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
		}

		public void Exit()
		{
			_inputReader.LeftMouseButtonPressed -= OnLeftMouseButtonClicked;

			if (_friendUnitActionStrategy != null) 
				_friendUnitActionStrategy.Disable();
		}

		private void OnLeftMouseButtonClicked()
		{
			if (_raycastService.Raycast(out Unit unit) == false)
				return;

			if (unit.PlatoonType == PlatoonType.Enemies)
				return;

			_friendUnitActionStrategy = new FriendActionStrategyAbilityTest(_battlefield, unit);
			_objectResolver.Inject(_friendUnitActionStrategy);
			_friendUnitActionStrategy.Enable();
		}
	}
}