using Infrastructure.Scripts.StateMachine.States;
using Source.Characters.Configs;
using Source.UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionScreenPanel : MonoBehaviour
{
    [field: SerializeField] public CharacterPreviewPanel CharacterPreviewPanel { get; private set; }
    [field: SerializeField] public CharacterSelectionPanel CharacterSelectionPanel { get; private set; }
    [field: SerializeField] public CharacterInfoPanel CharacterInfoPanel { get; private set; }

    [SerializeField] private Button _showButton;
    [SerializeField] private Button _startGameButton;
    
    private CharacterType _characterType;
    private LoadMainMenuState _loadMainMenuState;

    private void OnEnable()
    {
        _showButton.onClick.AddListener(OnShowButtonClicked);
        CharacterSelectionPanel.Clicked += OnCharacterSelectionPanelClicked;
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }
    
    private void OnDisable()
    {
        _showButton.onClick.RemoveListener(OnShowButtonClicked);
        CharacterSelectionPanel.Clicked -= OnCharacterSelectionPanelClicked;
        _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
    }

    public void Initialize(LoadMainMenuState loadMainMenuState) => 
        _loadMainMenuState = loadMainMenuState;

    private void OnCharacterSelectionPanelClicked(CharacterSkinItemView itemView)
    {
        GameObject characterPreviewPanel = Instantiate(itemView.CharacterItemConfig.CharacterModel);
        characterPreviewPanel.transform.rotation = new Quaternion(0, 180, 0, 0);
        CharacterPreviewPanel.Setup(characterPreviewPanel);
        
        CharacterInfoPanel.CreateItemViews(itemView.CharacterItemConfig);
        
        _startGameButton.gameObject.SetActive(true);
        _characterType = itemView.CharacterItemConfig.CharacterStaticData.Type;
    }
    
    private void OnStartGameButtonClicked()
    {
        
    }

    private void OnShowButtonClicked() => 
        Show();

    private void Show() => 
        CharacterSelectionPanel.Show();
}