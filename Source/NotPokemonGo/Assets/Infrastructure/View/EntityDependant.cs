using UnityEngine;

namespace Infrastructure.View
{
    public abstract class EntityDependant : MonoBehaviour
    {
        public EntityBehavior EntityView;
        
        public GameEntity Entity => EntityView != null ? EntityView.Entity : null;

        private void Awake()
        {
            if (!EntityView) 
                EntityView = GetComponent<EntityBehavior>();
        }
    }
}