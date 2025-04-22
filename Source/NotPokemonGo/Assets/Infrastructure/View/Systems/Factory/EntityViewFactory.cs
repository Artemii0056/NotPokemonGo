using Code.Infrastructure.AssetManagement;
using UnityEngine;
using Zenject;

namespace Infrastructure.View.Systems.Factory
{
    public class EntityViewFactory : IEntityViewFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IInstantiator _instantiator;
        private readonly Vector3 _farAway = new Vector3(-999, -999, 0);

        public EntityViewFactory(IAssetProvider assetProvider, IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }
        
        public EntityBehavior CreateViewForEntity(GameEntity entity)
        {
            EntityBehavior viewPrefab = _assetProvider.LoadAsset<EntityBehavior>(entity.ViewPath);
            var view = _instantiator.InstantiatePrefabForComponent<EntityBehavior>(
                viewPrefab,
                position: _farAway,
                rotation: Quaternion.identity,
                parentTransform: null);
            
            view.SetEntity(entity);
            
            return view;
        }
        
        public EntityBehavior CreateViewForEntityFromPrefab(GameEntity entity)
        {
            var view = _instantiator.InstantiatePrefabForComponent<EntityBehavior>(
                entity.ViewPrefab,
                position: _farAway,
                rotation: Quaternion.identity,
                parentTransform: null);
            
            view.SetEntity(entity);
            
            return view;
        }
    }
}