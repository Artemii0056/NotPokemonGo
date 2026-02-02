using System;
using System.Collections.Generic;
using QteSystem;
using UnityEngine;

namespace Abilities.Configs
{
    [Serializable]
    public class AbilityPhase
    {
        public AnimationClip AnimationClip;

        public QteType QteType;

        public List<PhaseSignalAction> SignalActions = new List<PhaseSignalAction>();

        public int AnimationCashName => Animator.StringToHash(AnimationClip.name);
    }
}