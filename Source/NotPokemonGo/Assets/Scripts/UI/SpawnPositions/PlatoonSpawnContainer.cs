using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;

namespace UI.SpawnPositions
{
    public class PlatoonSpawnContainer : MonoBehaviour
    {
        [SerializeField] private List<SpawnPoint> _spawnPoints;
        
        public int Count => _spawnPoints.Count;
        public  List<SpawnPoint> SpawnPoints => _spawnPoints.ToList();
    }
}