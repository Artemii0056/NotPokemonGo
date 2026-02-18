using Effects;
using Services.StaticDataServices;
using Spawners;
using Units;
using UnityEngine;

//с боем для милишников можно "уворачиваться" через те же qte 
namespace Statuses
{
    //Какие-то статусы тикают по шагам, какие-то по реал времени. 
    //Пример СЛОМА - при наложении нужно включить анимацию, подключить партикл? Добавить статус в бар + звук?
    //Пример отражения - проиграть анимацию + партикл перед игроком + воспроизвести звук отражения? 
    //BaseAnimations - сделать анимацию Слома. Одинаковая для всех 
    
    //Один интерфейс на статусы? Подумать. И несколько классов реализации. Реал тайм/пер терн/перн плеер терн. Разделить на 3 класса - Надо подумать
    
    //!!! Текущая таска - подвязать разные статусы к разному времени 
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
        }
        
        public string Name { get; protected set; }

        public float TickCount { get; protected set; }
        public StatusSetup Setup { get; protected set; }
        public Unit Target { get; protected set; }

        public bool IsPermanent { get; protected set; }
        public bool IsRefreshed { get; protected set; }

        public bool IsEnded => TickCount <= 0;
        
        public virtual void OnApply() //Реализовать сервис, который будет и с анимацией работать, и с партиклом, и с остальной лабудой
        {
            if (Setup.Type == StatusType.Bubble || Setup.Type == StatusType.Stun )
            {
                Debug.Log(Setup.Type );
                
                ParticleSystem particleSystemPrefab = _staticDataService.GetParticleByType(Setup.Type);
                _particleSpawner.Spawn(Target, particleSystemPrefab);
            }
        }

        public virtual void OnTick()
        {
            EffectSetup setupEffectSetup = Setup.EffectSetup;
            
            EffectInfo damageInfo = new EffectInfo(setupEffectSetup.Value, setupEffectSetup.TargetType, setupEffectSetup.Type, setupEffectSetup.DamageType);
            _effectResolver.ApplyEffect(Target, damageInfo);
        }

        public virtual void OnExpire()
        {
            Debug.Log("OnExpire" );
            _particleSpawner.Clear(Target);
        }

        public void Tick() //Разделить на два класса и добавить deltaTime
        {
            //Debug.Log("Tick " + TickCount);
            
            OnTick();
            //TickCount--;
            TickCount -= Time.deltaTime;
        }
        
        public void IncreaseTickCount(float tickCount) =>
            TickCount += tickCount;

        public void Refresh(Status status) =>
            TickCount = status.TickCount;
    }
}