using Characters;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenContainer CreateCharacterSelectionScreenPanel();
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
    }
}