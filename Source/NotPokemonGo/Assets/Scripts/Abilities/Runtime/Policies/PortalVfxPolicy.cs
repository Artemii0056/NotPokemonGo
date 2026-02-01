using Abilities.Configs;
using Abilities.MV;
using Units;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public sealed class PortalVfxPolicy : AbilityPolicyBase
    {
        private readonly ParticleSpawnType _spawnType;
        private readonly ParticleSystem _portalPrefab;

        private ParticleSystem _instance;

        public PortalVfxPolicy(AbilityModel model, ParticleSpawnType spawnType)
        {
            _spawnType = spawnType;
            _portalPrefab = ExtractPortalPrefab(model, spawnType);
        }

        public override void OnAbilityStart(AbilityContext context)
        {
            if (_instance != null)
                return;

            if (_portalPrefab == null)
                return;

            Unit owner = context?.Source;
            
            if (owner == null)
                return;

            Transform anchorPoint = FindAnchorPoint(owner, _spawnType);
            
            if (anchorPoint == null)
                return;

            _instance = Object.Instantiate(_portalPrefab, anchorPoint.position, Quaternion.identity, anchorPoint);
            _instance.Play();
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            return true;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            if (_instance != null)
                Object.Destroy(_instance.gameObject);

            _instance = null;
        }

        private static Transform FindAnchorPoint(Unit owner, ParticleSpawnType spawnType)
        {
            if (owner.AbilityAnchors == null)
                return null;

            for (int i = 0; i < owner.AbilityAnchors.Count; i++)
            {
                var anchor = owner.AbilityAnchors[i];
                
                if (anchor == null || anchor.spawnType != spawnType)
                    continue;

                if (anchor.Transforms == null || anchor.Transforms.Count == 0)
                    return null;

                return anchor.Transforms[0];
            }

            return null;
        }

        private static ParticleSystem ExtractPortalPrefab(AbilityModel model, ParticleSpawnType spawnType)
        {
            if (model?.Parts == null || model.Parts.Count == 0)
                return null;

            try
            {
                var firstPart = model.Parts[0];
                
                var firstPhase = firstPart?.AbilityPhases != null && firstPart.AbilityPhases.Count > 0
                    ? firstPart.AbilityPhases[0]
                    : null;

                var firstAction = firstPhase?.SignalActions != null && firstPhase.SignalActions.Count > 0
                    ? firstPhase.SignalActions[0]
                    : null;

                if (firstAction != null && firstAction.HasParticle && firstAction.ParticleSpawnType == spawnType)
                    return firstAction.ParticlePrefab;

                if (firstAction != null && firstAction.HasParticle && firstAction.ParticlePrefab != null)
                    return firstAction.ParticlePrefab;
            }
            catch
            {
                // игнор, есть fallback ниже
            }

            foreach (var part in model.Parts)
            {
                if (part?.AbilityPhases == null)
                    continue;

                foreach (var phase in part.AbilityPhases)
                {
                    if (phase?.SignalActions == null)
                        continue;

                    foreach (var action in phase.SignalActions)
                    {
                        if (action == null || !action.HasParticle)
                            continue;

                        if (action.ParticleSpawnType == spawnType && action.ParticlePrefab != null)
                            return action.ParticlePrefab;
                    }
                }
            }

            return null;
        }
    }
}
