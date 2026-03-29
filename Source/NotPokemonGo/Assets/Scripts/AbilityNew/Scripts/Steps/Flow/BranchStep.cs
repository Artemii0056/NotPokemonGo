using System;
using System.Collections.Generic;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Flow
{
    [Serializable]
    public class BranchStep : AbilityStepSO
    {
        public List<BranchCase> Cases = new();

        [SerializeReference]
        [SubclassSelector]
        public AbilityStepSO ElseStep;
    }
}