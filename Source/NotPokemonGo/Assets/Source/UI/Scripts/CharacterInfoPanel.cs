using Source.StaticData.CharactersCatalog.Scripts;
using UnityEngine;

namespace Source.UI.Scripts
{
    public class CharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;
        
        private CharacteristicItemView _characterInfoPanel;
        
        public void SetCharacteristicItemView(CharacteristicItemView characterItemView) => 
            _characterInfoPanel = characterItemView;

        public void CreateItemViews(CharacterItemConfig characterItemConfig)
        {
            
        }
    }
}