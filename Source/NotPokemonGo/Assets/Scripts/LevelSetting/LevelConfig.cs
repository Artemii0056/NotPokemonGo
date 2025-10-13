using System.Collections.Generic;
using Map;
using UnityEngine;

namespace LevelSetting
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "StaticData/" + nameof(LevelConfig))]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public MapType MapType { get; private set; }
        
        [SerializeField] private List<LevelPartSetup> _levelParts;
        
        public List<LevelPartSetup> LevelParts => _levelParts;
    }
}