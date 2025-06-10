using UnityEngine;

namespace UI
{
    public class CharacterPreviewPanel : MonoBehaviour
    {
        [SerializeField] private Transform _characterTransform;
        
        private GameObject _characterPreviewPanel;

        public void Setup(GameObject character)
        {
            if (_characterPreviewPanel != null) 
                Destroy(_characterPreviewPanel);
            
            _characterPreviewPanel = character;
        }
    }
}