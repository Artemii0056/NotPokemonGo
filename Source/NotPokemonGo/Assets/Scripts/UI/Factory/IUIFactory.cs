using Characters;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel();
        MainMenuUI CreateMainMenu();
        CharacterSelectionScreenPanel CreateCharacterSelectionPanel();
    }
}