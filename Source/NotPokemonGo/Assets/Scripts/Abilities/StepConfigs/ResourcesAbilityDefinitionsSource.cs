using Abilities.MV;
using UnityEngine;

namespace Abilities.StepConfigs
{
    /// <summary>
    /// Quick bootstrap for the branch: load registry from Resources/AbilityDefinitionsRegistry.
    /// Replace with Addressables/StaticData service later.
    /// </summary>
    public sealed class ResourcesAbilityDefinitionsSource : IAbilityDefinitionsSource
    {
        private const string ResourcesPath = "AbilityDefinitionsRegistry";

        private AbilityDefinitionsRegistry _registry;
        private bool _loaded;

        public bool TryGet(AbilityType type, out AbilityDefinitionConfig definition)
        {
            EnsureLoaded();

            if (_registry == null)
            {
                definition = null;
                return false;
            }

            return _registry.TryGet(type, out definition);
        }

        private void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;
            _registry = Resources.Load<AbilityDefinitionsRegistry>(ResourcesPath);

            if (_registry == null)
                Debug.LogWarning($"[Abilities] Step registry not found at Resources/{ResourcesPath}. Legacy fallback will be used.");
        }
    }
}
