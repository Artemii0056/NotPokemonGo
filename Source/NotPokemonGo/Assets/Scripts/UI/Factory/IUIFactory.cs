using Characters;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel(out UnitSelectionController unitSelectionController);
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
    }
}