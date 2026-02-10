using System.Collections.Generic;
using Abilities.Runtime;
using Armaments;
using Effects;
using Effects.Factory;
using Statuses;
using Statuses.Factory;
using UnityEngine;

namespace Factories.ArmamentViewFactories
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

        public Armament Create(ArmamentContext context, Transform transform)
        {
            List<EffectInfo> effects = _effectInfoFactory.Create(context.Setup.EffectsSetup);
            List<Status> statuses = _statusesFactory.Create(context.Setup.Statuses,  context.Source, context.Target);

            Armament armament = Object.Instantiate(context.Setup.ArmamentPrefab, transform.position, Quaternion.identity);
            armament.Initialize(effects,  statuses, context );
            
            return armament;
        }
    }
}