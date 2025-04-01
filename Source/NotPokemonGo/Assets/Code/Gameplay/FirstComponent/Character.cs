using UnityEngine;

namespace Code.Gameplay.FirstComponent
{
    public class Character
    {
        public void Factory(Transform transform)
        {
            EntityCreator.Create()
                .AddHealth(10f)
                .AddDamage(15f)
                .AddSpeed(5)
                .AddTransform(transform)
                ;
        }
    }
}