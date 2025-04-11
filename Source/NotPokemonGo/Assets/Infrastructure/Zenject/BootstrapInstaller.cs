using TestECS.Gameplay.Code.Features.Movement.Systems;
using TestECS.Gameplay.Input;
using Zenject;

namespace Infrastructure.Zenject
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapInstaller>().FromInstance(this).AsSingle();
                
            Container.Bind<ITimeService>().To<TimeService>().AsSingle();
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            Container.Bind<GameContext>().FromInstance(Contexts.sharedInstance.game).AsSingle();
            Container.Bind<Contexts>().FromInstance(Contexts.sharedInstance).AsSingle();
            
           // Container.BindInterfacesAndSelfTo<TimeService>().AsSingle(); //Bind - регистрация. 
        }
        
        //Методы поблочно. 
    }
}