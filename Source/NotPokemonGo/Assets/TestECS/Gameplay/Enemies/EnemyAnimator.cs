using TestECS.Gameplay.Code.Common.DamageTakenAnimator;
using UnityEngine;

namespace TestECS.Gameplay.Enemies
{
    public class EnemyAnimator : MonoBehaviour, IDamageTakenAnimator
    {
        public void PlayDamageTaken()
        {
            //Debug.Log("Play Damage Taken " + gameObject.name);
        }

        public void PlayDead()
        {
           // Debug.Log("Play Dead " + gameObject.name);
        }
    }
}