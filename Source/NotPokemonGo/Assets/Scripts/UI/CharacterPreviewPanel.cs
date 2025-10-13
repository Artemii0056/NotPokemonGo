using UnityEngine;

namespace UI
{
    public class CharacterPreviewPanel : MonoBehaviour
    {
        [SerializeField] private Transform _characterTransform;
        
        private Object _characterPreviewPanel;

        public void Setup(Object character)
        {
            if (_characterPreviewPanel != null) 
                Destroy(_characterPreviewPanel);
            
            _characterPreviewPanel = character;
        }
    }
}