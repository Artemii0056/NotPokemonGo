using System.Collections.Generic;
using Abilities;
using UnityEngine;

namespace Spawners
{
    public sealed class ParticleSpawner : IParticleSpawner
    {
        private readonly Dictionary<Units.Unit, Dictionary<ParticleSpawnType, AbilityAnchor>> _anchorsCache = new();
        private readonly Dictionary<Units.Unit, List<ParticleSystem>> _spawned = new();

        public void Spawn(Units.Unit owner, ParticleSpawnType spawnType, ParticleSystem prefab)
        {
            if (owner == null || prefab == null) return;

            var anchors = GetOrBuildAnchors(owner);

            if (!anchors.TryGetValue(spawnType, out var anchor) || anchor.Transforms == null || anchor.Transforms.Count == 0)
            {
                Debug.LogWarning($"No anchor for spawnType={spawnType} on unit={owner.name}");
                return;
            }

            var point = anchor.Transforms[0];
            var ps = Object.Instantiate(prefab, point.position, Quaternion.identity, point);
            ps.Play();

            if (!_spawned.TryGetValue(owner, out var list))
            {
                list = new List<ParticleSystem>(8);
                _spawned[owner] = list;
            }
            list.Add(ps);
        }

        public void Clear(Units.Unit owner)
        {
            if (owner == null) 
                return;
            
            if (!_spawned.TryGetValue(owner, out var list))
                return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var ps = list[i];
                if (ps != null) Object.Destroy(ps.gameObject);
                list.RemoveAt(i);
            }
        }

        private Dictionary<ParticleSpawnType, AbilityAnchor> GetOrBuildAnchors(Units.Unit unit)
        {
            if (_anchorsCache.TryGetValue(unit, out var map))
                return map;

            map = new Dictionary<ParticleSpawnType, AbilityAnchor>();
            
            foreach (var anchor in unit.AbilityAnchors)
            {
                if (!map.TryAdd(anchor.spawnType, anchor))
                    Debug.LogWarning($"Duplicate Anchor for {anchor.spawnType} on {unit.name}");
            }

            _anchorsCache[unit] = map;
            return map;
        }
    }
}