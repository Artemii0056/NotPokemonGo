using Infrastructure.DI.Scopes;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Installers
{
    public class MenuInstaller :  MonoInstaller
    {
        public LocationTypeView LocationTypeView;
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(LocationTypeView).AsImplementedInterfaces();
        }
    }
}