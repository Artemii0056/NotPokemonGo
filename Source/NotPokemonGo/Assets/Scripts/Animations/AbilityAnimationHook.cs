using Abilities;
using Services.StaticDataServices;
using Units;
using UnityEngine;
using VContainer;

namespace Animations
{
    public class AbilityAnimationHook : MonoBehaviour
    {
        public Unit unit;
        public Animator animator;
        private IAbilityProvider _abilityProvider;
        private IStaticDataService _staticDataService;

        [Inject]
        public void Initialize(IAbilityProvider abilityProvider, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
        }

        public void StartAnimationEffect()
        {
            PlayEffects();
        }

        public void MiddleAnimationEffect()
        {
            PlayEffects();
        }

        public void EndAnimationEffect()
        {
            PlayEffects();
        }

        private void PlayEffects()
        {
            AbilityConfig abilityConfig = _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);

            foreach (ParticleSystem particleSystem in abilityConfig.StartAnimationParticles)
            {
                Instantiate(particleSystem, unit.transform);
            }
        }

        // public void OnHit()
        // {
        //     // можно получить текущую анимацию и кто её использует
        //     Debug.Log("Ability Hit");
        //     unit.ApplyAnimationEvent();
        // }
        //
        // public void CreateStartParticle()
        // {
        //     Debug.Log("Ability Hit");
        // }
    }
}