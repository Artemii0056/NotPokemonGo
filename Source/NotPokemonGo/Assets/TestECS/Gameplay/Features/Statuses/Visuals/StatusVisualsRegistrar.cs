using Code.Gameplay.Common.Visuals.StatusVisuals;
using Infrastructure.View.Registrars;

namespace TestECS.Gameplay.Features.Statuses.Visuals
{
  public class StatusVisualsRegistrar : EntityComponentRegistrar
  {
    public StatusVisuals StatusVisuals;
    
    public override void RegisterComponents() => 
      Entity.AddStatusVisuals(StatusVisuals);

    public override void UnregisterComponents()
    {
      if (Entity.hasStatusVisuals)
        Entity.RemoveStatusVisuals();
    }
  }
}