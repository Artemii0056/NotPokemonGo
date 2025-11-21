using Services.AssetManagement;
using UI.DamageTextUI;
using UnityEngine;
using VContainer;

namespace Services.Spawners
{
  public class BattleTextUISpawner : BaseSpawner<BattleUIText>
  {
    private const string PrefabPath = "UI/AnimateTextUI";
    
    private BattleUIText _cachedPrefab;
    public BattleTextUISpawner(
      IObjectResolver objectResolver,
      IResourceLoader resourceLoader) 
      : base(objectResolver, resourceLoader)
    {
      
    }

    public BattleUIText Spawn(Vector3 position, float targetValue, Sprite icon)
    {
      BattleUIText battleUIText = ObjectPool.Get();

      battleUIText.Initialize(position,  targetValue, icon);
      battleUIText.AnimationEnded += OnAnimationEnded;
      return battleUIText;
    }

    protected override BattleUIText GetPrefab()
    {
      if (_cachedPrefab == null)
        _cachedPrefab = ResourceLoader.Load<BattleUIText>(PrefabPath);

      return _cachedPrefab;
    }

    private void OnAnimationEnded(BattleUIText battleUIText)
    {
      ObjectPool.Release(battleUIText);
    }
  }
}