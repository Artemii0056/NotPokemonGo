using Code.Entity;
using TestECS.Gameplay.Hero.Registrars;
using UnityEngine;
using Zenject;

namespace Infrastructure.View
{
    public class SelfInitializedEntityView : MonoBehaviour
    {
        public EntityBehavior EntityBehaviour;
        private IIdService _idService;

        [Inject]
        private void Constructor(IIdService idService) => 
            _idService = idService;

        public void Awake()
        {
            GameEntity gameEntity = CreateEntity.Empty().
               AddId(_idService.Next());
            
            EntityBehaviour.SetEntity(gameEntity);
        }
    }
}