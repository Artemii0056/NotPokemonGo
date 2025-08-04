using System.Collections.Generic;
using Characters.Configs;
using LevelSetting;

namespace Infrastructure.StateMachines
{
    public class LevelRuntimeDataPayload
    {
        private readonly LevelConfig _levelConfig;
        public List<UnitType> Units { get; private set; }
        public BattleInfoUI BattleInfoUI { get; private set; }
        private int _currentWaveIndex;

        public LevelRuntimeDataPayload(LevelConfig levelConfig, List<UnitType> units, BattleInfoUI battleInfoUI)
        {
            _levelConfig = levelConfig;
            BattleInfoUI = battleInfoUI;
            Units = units;
            _currentWaveIndex = -1;
        }

        public bool HasNextWave => _currentWaveIndex + 1 < _levelConfig.LevelParts.Count;

        public LevelPartSetup NextWave() =>
            _levelConfig.LevelParts[++_currentWaveIndex];
    }
}