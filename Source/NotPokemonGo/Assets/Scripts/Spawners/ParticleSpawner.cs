using System;
using System.Collections.Generic;
using Abilities;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spawners
{
    public sealed class ParticleSpawner : IParticleSpawner
    {
        private readonly Dictionary<Unit, List<ParticleSystem>> _spawned = new();
        private readonly Dictionary<Unit, Dictionary<ParticleSpawnType, AbilityAnchor>> _anchorsCache = new();

        public void Spawn(Unit owner, ParticleSpawnType spawnType, ParticleSystem prefab)
        {
            if (owner == null || prefab == null) 
                throw new NullReferenceException();

            Dictionary<ParticleSpawnType, AbilityAnchor> anchors = GetOrBuildAnchors(owner);

            if (!anchors.TryGetValue(spawnType, out var anchor) || anchor.Transforms == null || anchor.Transforms.Count == 0)
            {
                Debug.LogWarning($"No anchor for spawnType={spawnType} on unit={owner.name}");
                return;
            }

            Transform point = anchor.Transforms[0];
            ParticleSystem ps = Object.Instantiate(prefab, point.position, Quaternion.identity);
            ps.Play();

            if (!_spawned.TryGetValue(owner, out List<ParticleSystem> list))
            {
                list = new List<ParticleSystem>(8);
                _spawned[owner] = list;
            }
            
            list.Add(ps);
        }

        public void Spawn(Unit target, ParticleSystem prefab)
        {
            if (target == null || prefab == null) 
                throw new NullReferenceException();
            
            ParticleSystem ps = Object.Instantiate(prefab, target.transform.position, Quaternion.identity);
           // ps.gameObject.transform.localScale = target.transform.localScale;
            ps.Play();
            
            if (_spawned.TryGetValue(target, out List<ParticleSystem> list) == false)
            {
                list = new List<ParticleSystem>(8);
                _spawned[target] = list;
            }
            
            list.Add(ps);
        }

        public void Clear(Unit owner)
        {
            if (owner == null) 
                return;
            
            if (!_spawned.TryGetValue(owner, out var list))
                return;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var ps = list[i];
                
                if (ps != null) 
                    Object.Destroy(ps.gameObject);
                
                list.RemoveAt(i);
            }
        }

        private Dictionary<ParticleSpawnType, AbilityAnchor> GetOrBuildAnchors(Unit unit)
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