using System;
using Effects;
using UnityEngine.Serialization;

namespace Statuses
{
    [Serializable]
    public class StatusSetup
    {
        public EffectSetup EffectSetup;
        public StatusType Type;

        public float TargetTime;
        public float TickCount;
        
        public bool IsPermanent;
        public bool IsRefreshed;
    }
}