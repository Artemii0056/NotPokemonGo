using System;

namespace TestECS.Gameplay.Features.Abilities.Configs
{
    [Serializable]
    public class ProjectileSetup
    {
        public float Speed;
        public int Pierce;
        public float ContactRadius;
        public float Lifetime;
        
        public int FragmentsCount;
    }
}