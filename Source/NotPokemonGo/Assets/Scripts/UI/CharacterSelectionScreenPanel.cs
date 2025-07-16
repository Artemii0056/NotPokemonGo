using System;
using Characters;
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

        public Action ExitClicked;

        private void OnEnable()
        {
            UnitContainerPanel.Clicked += OnUnitSelected;
            
            ExitButton.onClick.AddListener(() => ExitClicked?.Invoke());
        }

        private void OnDisable()
        {
            UnitContainerPanel.Clicked -= OnUnitSelected;
            
            ExitButton.onClick.RemoveAllListeners();
        }

        private void OnUnitSelected(UnitSkinItemView itemView)
        {
            GameObject characterPreviewPanel = Instantiate(itemView.UnitItemConfig.CharacterModel);
            characterPreviewPanel.transform.rotation = new Quaternion(0, 180, 0, 0);
            CharacterPreviewPanel.Setup(characterPreviewPanel);

            UnitStatsPanel.CreateItemViews(itemView.UnitItemConfig);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UnitContainerPanel.Show();
        }

        public void Hide()
        {
            UnitContainerPanel.Hide();
            gameObject.SetActive(false);
        }
    }
}