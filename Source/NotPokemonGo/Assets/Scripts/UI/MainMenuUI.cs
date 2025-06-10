using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        private void OnEnable() => 
            _startButton.onClick.AddListener(OnButtonClick);

        private void OnDisable() => 
            _startButton.onClick.RemoveListener(OnButtonClick);

        public void OnButtonClick()
        {
            
        }
    }
}