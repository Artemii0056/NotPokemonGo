using Characters;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel(out ChooseUnitsForBattle chooseUnitsForBattle);
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
       // CharacterSelectionScreenPanel CreateCharacterSelectionPanel();
    }
}