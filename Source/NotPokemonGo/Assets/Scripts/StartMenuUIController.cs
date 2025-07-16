using UI;

public class StartMenuUIController
{
    private readonly StartScreenUI _startScreenUI;
    private readonly CharacterSelectionScreenPanel _characterSelectionScreenPanel;
    private readonly ChooseMapUI _chooseMapUI;

    public StartMenuUIController(
        StartScreenUI startScreenUI,
        CharacterSelectionScreenPanel characterSelectionScreenPanel,
        ChooseMapUI chooseMapUI)
    {
        _startScreenUI = startScreenUI;
        _characterSelectionScreenPanel = characterSelectionScreenPanel;
        _chooseMapUI = chooseMapUI;

        _startScreenUI.ShowHeroesClicked += ShowCharacterPanel;
        _startScreenUI.ChoosePlatoonClicked += ShowChooseMap;
    }

    private void ShowCharacterPanel()
    {
        _characterSelectionScreenPanel.Show();
        _startScreenUI.gameObject.SetActive(false);
        _characterSelectionScreenPanel.ExitClicked += ShowStartScreen;
    }

    private void ShowStartScreen()
    {
        _characterSelectionScreenPanel.Hide();
        _characterSelectionScreenPanel.gameObject.SetActive(false);
        _startScreenUI.gameObject.SetActive(true);
        _characterSelectionScreenPanel.ExitClicked -= ShowStartScreen;
    }

    private void ShowChooseMap()
    {
        _startScreenUI.gameObject.SetActive(false);
        _chooseMapUI.gameObject.SetActive(true);
    }

    public void Cleanup()
    {
        _startScreenUI.ShowHeroesClicked -= ShowCharacterPanel;
        _startScreenUI.ChoosePlatoonClicked -= ShowChooseMap;
        _characterSelectionScreenPanel.ExitClicked -= ShowStartScreen;
    }
}