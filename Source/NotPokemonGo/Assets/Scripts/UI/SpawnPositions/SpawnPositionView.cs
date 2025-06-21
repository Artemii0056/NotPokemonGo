using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace UI.SpawnPositions
{
    public class SpawnPositionView : MonoBehaviour
    {
        public List<SpawnPositionButton> SpawnPositionButtons;

        public event Action<SpawnPositionType> SpawnPositionChanged;
        
        private void OnEnable()
        {
            foreach (SpawnPositionButton spawnPositionButton in SpawnPositionButtons) 
                spawnPositionButton.OnClick += SelectSpawnPosition;
        }

        private void OnDisable()
        {
            foreach (SpawnPositionButton spawnPositionButton in SpawnPositionButtons) 
                spawnPositionButton.OnClick -= SelectSpawnPosition;
        }

        private void SelectSpawnPosition(SpawnPositionType spawnPositionType) => 
            SpawnPositionChanged?.Invoke(spawnPositionType);
    }
}