using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace LevelSetting
{
    [Serializable]
    public class LevelPartSetup
    {
        [SerializeField] private List<UnitConfig> _units = new List<UnitConfig>();
        
        public List<UnitConfig> Units => _units;
    }
}