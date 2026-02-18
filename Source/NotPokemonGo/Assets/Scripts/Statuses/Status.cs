using Effects;
using Services.StaticDataServices;
using Spawners;
using Units;
using UnityEngine;

//с боем для милишников можно "уворачиваться" через те же qte 
namespace Statuses
{
    //Пример СЛОМА - при наложении нужно включить анимацию, подключить партикл? Добавить статус в бар + звук?
    //BaseAnimations - сделать анимацию Слома. Одинаковая для всех 

    //Один интерфейс на статусы? Подумать. И несколько классов реализации. Реал тайм/пер терн/перн плеер терн. Разделить на 3 класса - Надо подумать

    public class Status //надо разделить на два вида. Первый тикающий, второй типо оглушение. 
    {
        private readonly IEffectResolver _effectResolver;
        private readonly IStaticDataService _staticDataService;
        private readonly IParticleSpawner _particleSpawner;

        public Status(
            StatusSetup setup,
            Unit target,
            IEffectResolver effectResolver,
            IStaticDataService staticDataService,
            IParticleSpawner particleSpawner)
        {
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;
            _staticDataService = staticDataService;
            _particleSpawner = particleSpawner;

            TickCount = Setup.TickCount;
            Duration = Setup.Duration;
        }

        public string Name { get; private set; }

        public float TickCount { get; private set; }
        public float Duration { get; private set; }
        public StatusSetup Setup { get; private set; }
        public Unit Target { get; private set; }

        public bool IsPermanent { get; private set; }
        public bool IsRefreshed { get; private set; }

        public bool IsEnded => TickCount <= 0;
        public bool IsRealtimeEnded => Duration <= 0;

        public void OnApply() //Реализовать сервис, который будет и с анимацией работать, и с партиклом, и с остальной лабудой
        {
            Debug.Log(Setup.Type);

            ParticleSystem particleSystemPrefab = _staticDataService.GetParticleByType(Setup.Type);
            _particleSpawner.Spawn(Target, particleSystemPrefab);
        }

        public void OnExpire()
        {
            Debug.Log("OnExpire");
            _particleSpawner.Clear(Target);
        }

        public void Tick() 
        {
            if (TickCount > 0)
                TickCount--;

            Duration -= Time.deltaTime;
        }

        public void OnTick()
        {
            EffectSetup setupEffectSetup = Setup.EffectSetup;

            EffectInfo damageInfo = new EffectInfo(setupEffectSetup.Value, setupEffectSetup.TargetType,
                setupEffectSetup.Type, setupEffectSetup.DamageType);
            
            _effectResolver.ApplyEffect(Target, damageInfo);
        }

        public void IncreaseTickCount(float tickCount) =>
            TickCount += tickCount;

        public void Refresh(Status status) =>
            TickCount = status.TickCount;
    }
}