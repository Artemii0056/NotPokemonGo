using Characters;
using UnityEngine;

namespace UI.Factory
{
    public interface IUIFactory
    {
        CharacterSelectionScreenContainer CreateCharacterSelectionScreenPanel();
        StartScreenUI CreateStartScreen();
        MainMenuUI CreateMainMenu();
        LoosePanel CreateLoosePanel();
        BattleInfoUI CreateBattleUIInfo();
        WinPanel CreateWinPanel();

        T CreatePanel<T>() where T : MonoBehaviour;
    }
}