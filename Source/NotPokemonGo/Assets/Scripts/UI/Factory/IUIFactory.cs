using Characters;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel();
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
        CharacterSelectionScreenPanel CreateCharacterSelectionPanel();
    }
}