using Infrastructure.StateMachines.GlobalStateMachine;

namespace Infrastructure.StateMachines
{
    public class ChoosePlatoonPayload
    {
        public readonly ChooseMapUI ChooseMapUI;
        public readonly IGameStateMachine GameStateMachine;

        public ChoosePlatoonPayload(ChooseMapUI chooseMapUI, IGameStateMachine gameStateMachine)
        {
            ChooseMapUI = chooseMapUI;
            GameStateMachine = gameStateMachine;
        }
    }
}