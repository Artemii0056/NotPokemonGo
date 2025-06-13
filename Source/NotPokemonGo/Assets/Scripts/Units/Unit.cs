using System.Collections.Generic;
using Characters.Configs.Stats;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();

        public void Initialize(List<StatSetup> statSetups)
        {
            foreach (var statSetup in statSetups)
            {
                _stats.Add(statSetup.StatType, statSetup);
            }
        }

        public void ChangeValue(StatType statType, float value)
        {
            _stats[statType].Modify(value);
        }
    }
}