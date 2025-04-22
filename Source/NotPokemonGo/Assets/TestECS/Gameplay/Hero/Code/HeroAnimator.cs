using TestECS.Gameplay.Code.Common.DamageTakenAnimator;
using UnityEngine;

namespace TestECS.Gameplay.Hero.Code
{
    public class HeroAnimator : MonoBehaviour, IDamageTakenAnimator
    {
        public void PlayMove()
        {
        }
    
        public void PlayIdle()
        {
        }

        public void PlayDied()
        {
            Debug.Log("Died");
        }

        public void PlayDamageTaken()
        {
            Debug.Log("Play Damage Taken");
        }
    }
}