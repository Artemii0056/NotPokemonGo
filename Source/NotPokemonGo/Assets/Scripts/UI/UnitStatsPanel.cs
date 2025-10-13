using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Characters;
using Characters.Configs;
using Stats;
using UnityEngine;

namespace UI
{
    public class UnitStatsPanel : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;
        [SerializeField] private List<StatUIInfo> _statsUIInfos = new List<StatUIInfo>();
        
        private CharacteristicItemView _characterInfoPanel;
        private List<CharacteristicItemView> _characteristicSkinItemViews = new List<CharacteristicItemView>();
        
        public void SetCharacteristicItemView(CharacteristicItemView characterItemView) => 
            _characterInfoPanel = characterItemView;

        public void CreateItemViews(UnitItemConfig unitItemConfig)
        {
            ClearItemViews();

            var conf = unitItemConfig.UnitConfig.Stats;

            foreach (var statsUIInfo in _statsUIInfos)
            {
                CharacteristicItemView itemView = Instantiate(_characterInfoPanel, _gridLayoutGroupTransform, false);
                
                StatConfig statConfig = conf.FirstOrDefault(x => x.StatsType == statsUIInfo.StatsType);
                
                StatUIInfo statUIInfo = _statsUIInfos.FirstOrDefault(x => x.StatsType == statConfig.StatsType);
                Sprite sprite = statUIInfo?.Image;
                
                itemView.Initialize(sprite, statConfig.Value.ToString(CultureInfo.InvariantCulture), statConfig.StatsType.ToString(CultureInfo.InvariantCulture));
                
                _characteristicSkinItemViews.Add(itemView);
            }
        }

        private void ClearItemViews()
        {
            foreach (var view in _characteristicSkinItemViews) 
                Destroy(view.gameObject);

            _characteristicSkinItemViews.Clear();
        }
    }
}