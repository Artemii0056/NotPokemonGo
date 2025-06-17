using System.Collections;
using System.Collections.Generic;
using Characters;
using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public class Platoon
    {
        private readonly List<Character> _characters;

        public Platoon(List<Character> characters)
        {
            _characters = characters;
        }

        private IEnumerator ApplayAbility()
        {
            while (true)
            {
                foreach (Character character in _characters)
                {
                    if (character.Step.IsReadyToAct)
                    {
                        character.UseAbility();
                        Debug.Log($"{character.Name} Use Ability");
                    }
                    else
                    {
                        Debug.LogError($"{character.Name} Try Use Ability");
                    }
                }
                yield return null;
            }
        }

        public bool IsAlive { get; private set; }

        public void Tick(float deltaTime)
        {
            foreach (Character character in _characters)
            {
                if (character.Step.IsReadyToAct)
                {
                    character.Step.ResetCurrentValue();
                }
                else
                {
                    character.UpdateStepCurrentValue(deltaTime);
                }
            }
        }

        private void VerifyIsAlive()
        {
            foreach (Character character in _characters)
            {
                if (character.IsAlive)
                {
                    IsAlive = true;
                    break;
                }
            }
        }
    }
}