using System;
using Effects;
using Services.AssetManagement;
using Services.Spawners;
using Services.StaticDataServices;
using Statuses;
using UI;
using Units;
using VContainer;

namespace Services.UITextServices
{
  public class UITextService : IDisposable
  {
    private readonly IEffectResolver _effectResolver;
    private readonly IStaticDataService _staticDataService;
    private readonly BattleTextUISpawner _battleTextUISpawner;

    public UITextService(
      IEffectResolver effectResolver, 
      IStaticDataService staticDataService, 
      IObjectResolver  objectResolver, 
      IResourceLoader resourceLoader)
    {
      _effectResolver = effectResolver;
      _staticDataService = staticDataService;
      
      //_effectResolver.EffectOnTargetCompleted += OnEffectOnTargetCompleted;
      _battleTextUISpawner = new BattleTextUISpawner(objectResolver, resourceLoader);
    }

    public void Dispose()
    {
      //_effectResolver.EffectOnTargetCompleted -= OnEffectOnTargetCompleted;
    }

    private void OnEffectOnTargetCompleted(EffectType effectType, Unit target, float targetValue)
    {
      UnitViewPanel viewPanel = target.GetComponentInChildren<UnitViewPanel>();

      switch (effectType)
      {
        case EffectType.Damage:
          _battleTextUISpawner.Spawn(viewPanel.transform.position, targetValue, _staticDataService.GetStatusIcon(StatusType.Damage));
          break;
        
        case EffectType.Heal:
          _battleTextUISpawner.Spawn(viewPanel.transform.position, targetValue, _staticDataService.GetStatusIcon(StatusType.Heal));
          break;
        
        default:
          throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
      }
    }
  }
}