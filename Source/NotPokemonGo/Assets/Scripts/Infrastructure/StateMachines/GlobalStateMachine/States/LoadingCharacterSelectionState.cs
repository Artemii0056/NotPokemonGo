using Infrastructure.StateMachines.States.Interfaces;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingCharacterSelectionState : IPayloadedState<StartMenuPayload>
    {
        private StartScreenUI _startScreenUI;
        
        public void Enter(StartMenuPayload payload)
        {
            _startScreenUI = payload.UI;

            StartMenuUIController startMenuUIController = new StartMenuUIController(payload.UI, payload.UI._characterSelectionScreenPanel, payload.UI._chooseMapUI);
        }

        public void Exit()
        {
        }
    }
}