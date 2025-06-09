using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Source.StaticData.CharactersCatalog.Scripts;
using UnityEngine;

namespace Source.UI.Scripts
{
    public class CharacterInfoPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;
        [SerializeField] private List<StatUIInfo> _statsUIInfos = new List<StatUIInfo>();
        
        private CharacteristicItemView _characterInfoPanel;
        private List<CharacteristicItemView> _characteristicSkinItemViews = new List<CharacteristicItemView>();
        
        public void SetCharacteristicItemView(CharacteristicItemView characterItemView) => 
            _characterInfoPanel = characterItemView;

        public void CreateItemViews(CharacterItemConfig characterItemConfig)
        {
            ClearItemViews();

            foreach (var stat in characterItemConfig.CharacterStaticData.Stats)
            {
                CharacteristicItemView itemView = Instantiate(_characterInfoPanel, _gridLayoutGroupTransform, false);

                StatUIInfo statUIInfo = _statsUIInfos.FirstOrDefault(x => x.StatsType == stat.StatsType);
                Sprite sprite = statUIInfo?.Image;

                itemView.Initialize(sprite, stat.Value.ToString(CultureInfo.InvariantCulture));

                _characteristicSkinItemViews.Add(itemView);
            }
        }

        private void ClearItemViews()
        {
            foreach (var view in _characteristicSkinItemViews)
            {
                Destroy(view.gameObject);
            }

            _characteristicSkinItemViews.Clear();
        }
    }
}