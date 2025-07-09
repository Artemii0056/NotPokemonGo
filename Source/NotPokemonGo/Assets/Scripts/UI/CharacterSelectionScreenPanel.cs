using Characters;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Services.StaticDataServices;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterSelectionScreenPanel : MonoBehaviour
    {
        [field: SerializeField] public CharacterPreviewPanel CharacterPreviewPanel { get; private set; }
        [field: SerializeField] public UnitContainerPanel UnitContainerPanel { get; private set; }
        [field: SerializeField] public UnitStatsPanel UnitStatsPanel { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }

        [SerializeField] private Button _showButton;
        [SerializeField] private Button _startGameButton;
        
        private IStaticDataService _staticDataService;
    
        private LoadMainMenuState _loadMainMenuState;

        private void OnEnable()
        {
            UnitContainerPanel.Clicked += OnUnitContainerPanelClicked;
        }
    
        private void OnDisable()
        {
            UnitContainerPanel.Clicked -= OnUnitContainerPanelClicked;
        }

        private void OnUnitContainerPanelClicked(UnitSkinItemView itemView)
        {
            GameObject characterPreviewPanel = Instantiate(itemView.UnitItemConfig.CharacterModel);
            characterPreviewPanel.transform.rotation = new Quaternion(0, 180, 0, 0);
            CharacterPreviewPanel.Setup(characterPreviewPanel);
        
            UnitStatsPanel.CreateItemViews(itemView.UnitItemConfig);
        
            _startGameButton.gameObject.SetActive(true);
        }
    
        public void Show() => 
            UnitContainerPanel.Show();

        public void Hide() =>
            UnitContainerPanel.Hide();
    }
}