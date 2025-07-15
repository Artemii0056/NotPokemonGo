using Characters;
using Characters.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitSelectionController : MonoBehaviour
    {
        [SerializeField] private UnitContainerUI _unitContainerUI;
        [field: SerializeField] public UnitContainerPanel UnitContainerPanel { get; private set; }

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;

        private void OnEnable()
        {
            UnitContainerPanel.Show();
            _unitContainerUI.Show();
            
            UnitContainerPanel.Clicked += OnUnitClicked;
            _startButton.onClick.AddListener(OnStartButtonClicked);
            
            _unitContainerUI.Initialize(this);
        }

        private void OnDisable()
        {
            UnitContainerPanel.Clicked -= OnUnitClicked;
            _startButton.onClick.RemoveListener(OnStartButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            var types = _unitContainerUI.GetUnitTypes();

            foreach (var type in types)
            {
                Debug.Log(type);
            }
        }

        public void ReleaseButtonByType(UnitType unitType)
        {
            UnitContainerPanel.Release(unitType);
        }

        private void OnUnitClicked(UnitSkinItemView unitSkinItemView)
        {
            UnitSkinItemViewForChoose viewForChoose = _unitContainerUI.GetFree();
            
            viewForChoose.Initialize(unitSkinItemView.UnitItemConfig.Type, unitSkinItemView.UnitItemConfig.ContentImage);
        }
    }
}