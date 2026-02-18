using Effects;
using UnityEngine;

namespace Statuses
{
    [CreateAssetMenu(fileName = nameof(StatusSetup), menuName = "StaticData/" + nameof(StatusSetup))]
    public class StatusSetup : ScriptableObject
    {
        public EffectSetup EffectSetup;
        public StatusType Type;
        public StatusUpdateType UpdateType;

        public float TargetTime;
        public float TickCount;
        
        public bool IsPermanent;
        public bool IsRefreshed;
    }
}