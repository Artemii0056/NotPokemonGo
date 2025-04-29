using System;
using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Hero.Registrars;

namespace TestECS.Gameplay.Features.Effects.Factory
{
    public class EffectFactory : IEffectFactory
    {
        private readonly IIdService _idService;

        public EffectFactory(IIdService idService) =>
            _idService = idService;

        public GameEntity CreateEffect(EffectSetup setup, int producerId, int targetId)
        {
            switch (setup.EffectTypeId)
            {
                case EffectTypeId.Unknown:
                    break;
                case EffectTypeId.Damage:
                    return CreateDamageEffect(producerId, targetId, setup.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            throw new Exception($"Unknown effect type: {setup.EffectTypeId}");
        }

        private GameEntity CreateDamageEffect(int producerId, int targetId, float value)
        {
             return CreateEntity.Empty()
                .AddId(_idService.Next())
                .With(x => x.isEffect = true)
                .With(x => x.isDamageEffect = true)
                .AddEffectValue(value)
                .AddProducerId(producerId)
                .AddTargetId(targetId)
                ;
        }
    }
}