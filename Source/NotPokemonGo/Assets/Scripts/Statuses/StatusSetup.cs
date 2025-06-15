using System;
using Effects;

namespace Statuses
{
    [Serializable]
    public class StatusSetup
    {
        public EffectSetup EffectSetup;

        public float Duration;
        public float TargetTime;
        public float TickCount;
        public bool IsPermanent;
    }
}