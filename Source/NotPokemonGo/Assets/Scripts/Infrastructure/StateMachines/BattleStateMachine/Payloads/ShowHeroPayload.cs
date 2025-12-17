using UI;

namespace Infrastructure.StateMachines.BattleStateMachine.Payloads
{
    public class ShowHeroPayload
    {
        public readonly CharacterSelectionScreenContainer CharacterSelectionScreenContainer;

        public ShowHeroPayload(CharacterSelectionScreenContainer characterSelectionScreenContainer) => 
            CharacterSelectionScreenContainer = characterSelectionScreenContainer;
    }
}