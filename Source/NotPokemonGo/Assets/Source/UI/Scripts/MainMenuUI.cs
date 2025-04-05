using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Scripts
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