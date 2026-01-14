using System;
using Abilities.Signals;
using Armaments;
using Castaments;
using UnityEngine;

namespace Abilities.Configs
{
    [Serializable]
    public class PhaseSignalAction
    {
        public PhaseSignal Signal;

        [Header("Optional payloads")]
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;

        [Header("Targeting")]
        public TargetMode TargetMode = TargetMode.Single;

        public bool HasArmament => ArmamentSetup != null && ArmamentSetup.HasSetupData;
        public bool HasCastament => CastamentSetup != null && CastamentSetup.HasSetupData;
    }
}