using System;
using System.Collections.Generic;
using Services.StaticDataServices;
using UnityEngine;

namespace Animations
{
    public class AbilityAnimationControllerBase : MonoBehaviour
    {
        public Animator _animator;
        private bool _isPlaying;
        
        public event Action ParticleSystem1Started;
        public event Action ParticleSystem2Started;
        public event Action ParticleSystem3Started;

        public void Play(string animationName)
        {
            if (_isPlaying)
            {
                throw new Exception();
            }

            _isPlaying = true;
            _animator.Play(animationName);
        }

        public void FlagParticleSystem1()
        {
            ParticleSystem1Started?.Invoke();
        }

        public void FlagParticleSystem2()
        {
            ParticleSystem2Started?.Invoke();
        }

        public void FlagParticleSystem3()
        {
            ParticleSystem3Started?.Invoke();
        }
    }

    public interface IParticleSystemFactory
    {
        void Create(List<ParticleSystem> particleSystems, Vector3 position, Quaternion rotation);
    }

    public class ParticleSystemFactory : IParticleSystemFactory
    {
        private readonly IStaticDataService _staticDataService;

        public ParticleSystemFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public void Create(List<ParticleSystem> particleSystems, Vector3 position, Quaternion rotation)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                GameObject.Instantiate(particleSystem,  position, rotation);
            }
        }
    }
}