using Infrastructure.StateMachines.GlobalStateMachine;
using UI;

namespace Infrastructure.StateMachines
{
    public class ShowHeroPayload
    {
        public readonly CharacterSelectionScreenContainer CharacterSelectionScreenContainer;

        public ShowHeroPayload(CharacterSelectionScreenContainer characterSelectionScreenContainer) => 
            CharacterSelectionScreenContainer = characterSelectionScreenContainer;
    }
}