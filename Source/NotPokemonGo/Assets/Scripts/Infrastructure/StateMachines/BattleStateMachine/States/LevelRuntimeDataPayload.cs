using System.Collections.Generic;
using Characters.Configs;
using LevelSetting;
using UI;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class LevelRuntimeDataPayload
    {
        private readonly LevelConfig _levelConfig;
        public List<UnitType> Units { get; private set; }
        public BattleInfoUI BattleInfoUI { get; private set; }
        public int CurrentWaveIndex { get; private set; }

        public LevelRuntimeDataPayload(LevelConfig levelConfig, List<UnitType> units, BattleInfoUI battleInfoUI)
        {
            _levelConfig = levelConfig;
            BattleInfoUI = battleInfoUI;
            Units = units;
            CurrentWaveIndex = -1;
        }

        public bool HasNextWave => CurrentWaveIndex + 1 < _levelConfig.LevelParts.Count;

        public LevelPartSetup NextWave() =>
            _levelConfig.LevelParts[++CurrentWaveIndex];
    }
}