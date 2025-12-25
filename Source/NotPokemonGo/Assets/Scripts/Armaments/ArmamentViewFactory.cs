using System.Collections.Generic;
using Effects;
using Effects.Factory;
using Statuses;
using Statuses.Factory;
using UnityEngine;

namespace Armaments
{
    public class ArmamentViewFactory : IArmamentViewFactory
    {
        private readonly IStatusesFactory _statusesFactory;
        private readonly IEffectInfoFactory _effectInfoFactory;

        public ArmamentViewFactory(
            IStatusesFactory statusesFactory, 
            IEffectInfoFactory effectInfoFactory)
        {
            _statusesFactory = statusesFactory;
            _effectInfoFactory = effectInfoFactory;
        }

        public Armament Create(ArmamentContext context)
        {
            List<EffectInfo> effects = _effectInfoFactory.Create(context.Setup.EffectsSetup);
            List<Status> statuses = _statusesFactory.Create(context.Setup.Statuses,  context.Source, context.Target);

            Armament armament = Object.Instantiate(context.Setup.ArmamentPrefab, context.Source.abilityPos.position, Quaternion.identity);
            armament.Initialize(effects,  statuses,  context.Source, context.Target, context.Setup, context.Flying);
            
            return armament;
        }
    }
}