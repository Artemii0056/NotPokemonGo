using Code.Gameplay.Common.Collisions;
using Infrastructure.View.Registrars;
using UnityEngine;
using Zenject;

namespace Infrastructure.View
{
    public class EntityBehavior : MonoBehaviour, IEntityView
    {
        private GameEntity _entity;
        private ICollisionRegistry _collisionRegistry;

        public GameEntity Entity => _entity;

        [Inject]
        private void Constructor(ICollisionRegistry collisionRegistry)
        {
            _collisionRegistry = collisionRegistry;
        }

        public void SetEntity(GameEntity entity)
        {
            _entity = entity;
            _entity.AddView(this);
            _entity.Retain(this);


            foreach (IEntityComponentRegistrar registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
                registrar.RegisterComponents();

            foreach (Collider collider in  GetComponentsInChildren<Collider>(true)) 
                _collisionRegistry.Register(collider.GetInstanceID(), _entity);
        }

        public void ReleaseEntity()
        {
            foreach (IEntityComponentRegistrar registrar in GetComponentsInChildren<IEntityComponentRegistrar>())
                registrar.UnregisterComponents();
            
            foreach (Collider collider in  GetComponentsInChildren<Collider>(true)) 
                _collisionRegistry.Unregister(collider.GetInstanceID());

            _entity.Release(this);
            _entity = null;
        }
    }
}