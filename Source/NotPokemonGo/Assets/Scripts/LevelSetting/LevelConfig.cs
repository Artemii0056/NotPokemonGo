using System.Collections.Generic;
using UnityEngine;

namespace LevelSetting
{
    [CreateAssetMenu(fileName = nameof(LevelConfig), menuName = "StaticData/" + nameof(LevelConfig))]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelType LevelType { get; private set; }
        
        [SerializeField] private List<LevelPartSetup> _levelParts;
        
        public List<LevelPartSetup> LevelParts => _levelParts;
    }
}