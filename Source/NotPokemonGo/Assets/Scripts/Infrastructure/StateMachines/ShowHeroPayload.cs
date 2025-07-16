using Infrastructure.StateMachines.GlobalStateMachine;
using UI;

namespace Infrastructure.StateMachines
{
    public class ShowHeroPayload
    {
        public readonly CharacterSelectionScreenPanel CharacterSelectionScreenPanel;
        public readonly IGameStateMachine GameStateMachine;

        public ShowHeroPayload(CharacterSelectionScreenPanel characterSelectionScreenPanel,
            IGameStateMachine gameStateMachine)
        {
            CharacterSelectionScreenPanel = characterSelectionScreenPanel;
            GameStateMachine = gameStateMachine;
        }
    }
}