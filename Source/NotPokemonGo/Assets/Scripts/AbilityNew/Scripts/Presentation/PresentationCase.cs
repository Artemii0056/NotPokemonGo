using System;
using System.Collections.Generic;
using AbilityNew.Scripts.Presentation.Conditions;
using UnityEngine;

namespace AbilityNew.Scripts.Presentation
{
    [Serializable]
    public sealed class PresentationCase
    {
        [SerializeReference]
        [SubclassSelector]
        public PresentationCondition Condition;

        [SerializeReference]
        [SubclassSelector]
        public List<PresentationStep> Steps = new();
    }
}