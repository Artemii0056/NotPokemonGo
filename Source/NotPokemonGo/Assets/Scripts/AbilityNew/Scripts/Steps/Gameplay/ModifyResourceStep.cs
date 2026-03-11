using System;
using AbilityNew.Scripts.Configs;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class ModifyResourceStep : AbilityStepSO
    {
        public int Value; //TODO Модифицирует конкретный стат
    }
}