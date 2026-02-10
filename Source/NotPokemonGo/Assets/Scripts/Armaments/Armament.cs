using System.Collections.Generic;
using System.Linq;
using Effects;
using Statuses;
using UnityEngine;
using Unit = Units.Unit;

namespace Armaments
{
    public class Armament : MonoBehaviour
    {
        private List<EffectInfo> _effectInfo;
        private List<Status> _statuses;

        public Unit Source { get; private set; }
        public Unit Target { get; private set; }
        public List<EffectInfo> Effects =>  _effectInfo.ToList();
        public List<Status> Statuses => _statuses.ToList();
        public ArmamentFlyingType FlyingType { get; private set; } = ArmamentFlyingType.Direct;
        public ArmamentContext Context { get; private set; }
        public ArmamentSetup Setup { get;  private set; }

        public void Initialize(List<EffectInfo> effectInfo, List<Status> statuses, ArmamentContext context)
        {
            _statuses = statuses;
            _effectInfo = effectInfo;
            Source = context.Source;
            Target = context.Target;
            Setup = context.Setup;
            FlyingType =context.FlyingType;
            Context = context;
        }
    }
}