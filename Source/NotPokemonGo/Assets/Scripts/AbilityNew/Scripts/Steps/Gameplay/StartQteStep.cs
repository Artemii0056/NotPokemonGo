using System;
using AbilityNew.Scripts.Configs;
using QteSystem;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class StartQteStep : AbilityStepSO
    {
        public QteType Type;

        public float Duration = 2f;
        
        public QteOutcomeMode OutcomeMode = QteOutcomeMode.Ternary;
    }
}