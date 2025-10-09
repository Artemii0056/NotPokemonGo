using System;
using Infrastructure.StateMachines;
using LevelSetting;
using Platoons;
using Services.BattleUnitContainers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.BattleSessionService
{
    public class BattlefieldSessionService : IDisposable, IBattlefieldSessionService
    {
        private readonly IBattlefieldFactory _battlefieldFactory;
        private readonly IUnitReadyService _unitReadyService;

        private Battlefield _currentBattlefield;
        private GameObject _battlefieldRoot;

        public BattlefieldSessionService(IBattlefieldFactory battlefieldFactory, IUnitReadyService unitReadyService)
        {
            _battlefieldFactory = battlefieldFactory;
            _unitReadyService = unitReadyService;
        }

        public Battlefield StartNewBattle(LevelRuntimeDataPayload levelData, LevelPartSetup wave, Platoon survivors = null)
        {
            Cleanup();

            if (levelData == null)
                throw new ArgumentNullException(nameof(levelData));

            if (wave == null)
                throw new ArgumentNullException(nameof(wave));
            
            Battlefield battlefield = survivors == null
                ? _battlefieldFactory.Create(levelData.Units, wave, out _battlefieldRoot)
                : _battlefieldFactory.Create(levelData.Units, survivors, wave, out _battlefieldRoot);
            
            _unitReadyService.SetPlatoons( battlefield.HeroesPlatoon, battlefield.EnemyPlatoon);

            _currentBattlefield = battlefield;

            return _currentBattlefield;
        }

        public void Cleanup()
        {
            if (_currentBattlefield == null)
                return;
            
            _unitReadyService.Discard();

            if (_battlefieldRoot != null)
            {
                Object.Destroy(_battlefieldRoot);
                _battlefieldRoot = null;
            }

            _currentBattlefield = null;
        }

        public void Dispose() => 
            Cleanup();
    }
}