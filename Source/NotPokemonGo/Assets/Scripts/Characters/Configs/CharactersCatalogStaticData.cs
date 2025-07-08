using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Configs
{
    [CreateAssetMenu(fileName = "Catalog", menuName = "StaticData/Catalog")]
    public class CharactersCatalogStaticData : ScriptableObject
    {
        [SerializeField] private List<UnitItemConfig> _characterItemConfigs;

        public IEnumerable<UnitItemConfig> CharacterItemConfigs => _characterItemConfigs;

        public void OnValidate()
        {
            var duplicate = _characterItemConfigs.GroupBy(item => item.ContentImage)
                .Where(group => group.Count() > 1);
        
            if (duplicate.Any())
                throw new Exception($"Duplicate sprite found: {duplicate}");
        }
    }
}