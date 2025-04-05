using System.Collections.Generic;
using UnityEngine;

namespace Source.StaticData.CharactersCatalog
{
    [CreateAssetMenu(fileName = "Catalog", menuName = "StaticData/Catalog")]
    public class CharactersCatalogStaticData : ScriptableObject
    {
        public List<CharacterItemConfig> Characters;
    }
}