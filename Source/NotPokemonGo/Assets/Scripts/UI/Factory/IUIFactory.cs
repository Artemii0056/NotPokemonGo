using Characters;
using UI.DodgeUI;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenContainer CreateCharacterSelectionScreenPanel();
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
        LoosePanel CreateLoosePanel();
        BattleInfoUI CreateBattleUIInfo();
        IDodgeView CreateDodgeView();
    }
}