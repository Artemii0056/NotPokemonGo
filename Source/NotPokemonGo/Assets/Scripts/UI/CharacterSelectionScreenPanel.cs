using System;
using Characters;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Services.StaticDataServices;
using Unity.VisualScripting;
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

        public Action ExitClicked;

        private void OnEnable() => 
            UnitContainerPanel.Clicked += OnUnitSelected;

        private void OnDisable() => 
            UnitContainerPanel.Clicked -= OnUnitSelected;

        private void OnUnitSelected(UnitSkinItemView itemView)
        {
            GameObject characterPreviewPanel = Instantiate(itemView.UnitItemConfig.CharacterModel);
            characterPreviewPanel.transform.rotation = new Quaternion(0, 180, 0, 0);
            CharacterPreviewPanel.Setup(characterPreviewPanel);

            UnitStatsPanel.CreateItemViews(itemView.UnitItemConfig);

            _startGameButton.gameObject.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UnitContainerPanel.Show();
            ExitButton.onClick.AddListener(Hide);
        }

        public void Hide()
        {
            ExitButton.onClick.RemoveListener(Hide);
            UnitContainerPanel.Hide();
            gameObject.SetActive(false);
            ExitClicked?.Invoke();
        }
    }
}